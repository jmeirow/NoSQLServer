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
using NoSQLServer.DBWriterService;




namespace NoSQLServer.Repository
{
    public class RepositoryBase
    {
        protected SqlCommand command = null;
        protected WriteRepository writeRepo = new WriteRepository();
        protected ReadRepository readRepo = new ReadRepository();
        protected BinaryFormatter formatter = new BinaryFormatter();

        public static readonly string Select1 = "select AggregateID, Data from {1}.Aggregates where AggregateTypeID = {0}";
        public static readonly string Select2 = "select IsNull(AggregateTypeID,-1) from NoSQLServer_MASTER.AggregateTypes where FullyQualifiedTypeName = '{0}'";
        public static readonly string Select3 = "select IsNull(Shard,'') from NoSQLServer_MASTER.AggregateTypes where FullyQualifiedTypeName = '{0}'";
        public long AggregateTypeID;


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


        public List<T> GetAll<T>() where T : IAggregateBase 
        {
            List<T> results = new List<T>();

            AggregateTypeInfo info = AggregateType.GetInfo(typeof(T).FullName);
            long typeID = info.ID;
            string Shard = info.Shard;



            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            {
                conn.Open();
                using (SqlCommand command = conn.CreateCommand())
                {

                    command.CommandText = string.Format(Select1, typeID, Shard);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        byte[] data = null;
                        while (reader.Read())
                        {
                            data = new byte[1];
                            long ID = reader.GetInt64(0);
                            data = (byte[])reader.GetValue(1);
                            using (MemoryStream stream = new MemoryStream(data))
                            {
                                T agg = (T)formatter.Deserialize(stream);
                                agg.AggregateID = ID;
                                results.Add(agg);
                            }
                        }
                    }
                }
            }

            return results;
        }


        public RepositoryBase(object domainObject)
        {
            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(Select2, domainObject.GetType().FullName);
                    AggregateTypeID = (long)cmd.ExecuteScalar();
                    cmd.CommandText = string.Format(Select3, domainObject.GetType().FullName);
                    Shard = ((string)cmd.ExecuteScalar()).Trim();
                }
            }
        }


        public long Save(AggregateBase aggregate)
        {
            var writer = new DBWriter();
            return writer.Save(aggregate, Shard, AggregateTypeID);
        }


        public string Submit(AggregateBase aggregate)
        {
            Guid guid = Guid.NewGuid();
            aggregate.AggregateTypeID = AggregateTypeID;

            var submitter = new Submitter(aggregate, Shard, guid, AggregateTypeID);
            //submitter.Submit();
            ThreadStart start = new ThreadStart(submitter.Submit);
            Thread t = new Thread(start);
            t.Start();
            while (!t.IsAlive) ;
            Thread.Sleep(1);
            t.Join();
            return guid.ToString();
        }


        public void Delete(AggregateBase aggregate)
        {
            var writer = new DBWriter();
            writer.Delete(aggregate, Shard);

        }
    }
}
