using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NoSQLServer; //.Adapters
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
 


namespace NoSQLServer.DBWriterService
{
    public class DBWriter
    {

        protected WriteRepository writeRepo = new WriteRepository();

        protected ReadRepository readRepo = new ReadRepository();

        protected BinaryFormatter formatter = new BinaryFormatter();
       
        private long aggregateTypeID;

        public long AggregateTypeID
        {
            get {
                return aggregateTypeID;
            }
            private set
            {
                aggregateTypeID = value;
            }
        }

        private string shard = "";

        public string Shard
        {
            get
            {
                return shard;
            }
            private set
            {
                shard = value;
            }
        }

        public void Submit(AggregateBase aggregate, string Shard)
        {
        }


        public long Save(AggregateBase aggregate, string Shard, long AggregateTypeID)
        {
            long ID = 0;
            try
            {
                aggregate.AggregateTypeID = AggregateTypeID;

                if (aggregate.AggregateID == 0)
                {
                    aggregate.VersionNumber = 1;
                }

                AggregateStore store = aggregate.ToAggregateStore();

                if (aggregate.AggregateID == 0)
                {
                    ID = Add(store, Shard); 
                    aggregate.AggregateID = ID;
                    store = aggregate.ToAggregateStore();
                    Update(aggregate, store, Shard); 
                }
                else
                {
                    Update(aggregate,store, Shard); 
                    ProcessCascadingUpdates(aggregate,store, Shard);   
                }

                if (HasIndexes(aggregate))
                {
                    DeleteIndexes(store, Shard);
                    RebuildIndexes(aggregate, store, Shard);
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(
                    @"Save failed: 
                    {0}
                    {1},", e.Message, e.InnerException.Message));
            }
            return ID;
        }

        public void Delete(AggregateBase aggregate, string Shard)
        {
            try
            {
                writeRepo.GetDeleteAggregate().Execute(aggregate.AggregateID, Shard);
            }
            catch (Exception e)
            {
                throw new Exception("Failed in 'Delete'", e);
            }
        }

        private bool HasIndexes(AggregateBase aggregate)
        {
            return aggregate.HasIndexes();
        }

        private long Add(AggregateStore store, string Shard)
        {
            try
            {
                return writeRepo.GetInsertAggregate().Execute(store, Shard);
            }
            catch (Exception e)
            {
                throw new Exception("Failed in 'Add'", e);
            }
        }

        private void Update(AggregateBase aggregate, AggregateStore store, string Shard)
        {
            try
            {
                writeRepo.GetUpdateAggregate().Execute(store, aggregate.GetUniqueKey(), Shard);
            }
            catch (Exception e)
            {
                throw new Exception("Failed in 'Update'", e);
            }
        }

        private void RebuildIndexes(AggregateBase aggregate, AggregateStore store, string Shard)
        {
            try
            {
                Dictionary<string, string> indexes = aggregate.GetIndexes();
                foreach (string key in indexes.Keys)
                {
                    writeRepo.GetInsertAggregateIndex().Execute(store, key, indexes[key], Shard);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed in 'RebuildIndexes'", e);
            }
        }

        private void DeleteIndexes(AggregateStore store, string Shard)
        {
            try
            {
                writeRepo.GetDeleteAggregateIndexes().Execute(store, Shard);
            }
            catch (Exception e)
            {
                throw new Exception("Failed in 'DeleteIndexes'", e);
            }
        }



        private void ProcessCascadingUpdates(AggregateBase aggregate, AggregateStore store, string Shard)
        {

            //
            QueueWriter queueWriter = new QueueWriter(aggregate, store, Shard);
            queueWriter.Send();
            Thread t = new Thread(queueWriter.Send);
            t.Start();
        }
    }
}
