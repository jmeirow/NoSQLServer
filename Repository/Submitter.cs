using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQLServer; 
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Messaging;



namespace NoSQLServer.Repository
{

    public class Submitter
    {
        protected BinaryFormatter formatter = new BinaryFormatter();

        AggregateBase aggregate = null;
        string Shard = string.Empty;
        String SubmissionID = string.Empty;
        Type ClassType = null;
        long AggregateTypeID = 0;
    
        
        public Submitter(AggregateBase aggregate, string Shard, Guid guid, long AggregateTypeID)
        {
            this.aggregate = aggregate;
            this.Shard = Shard;
            this.SubmissionID = guid.ToString();
            this.ClassType = aggregate.GetType();
            this.AggregateTypeID = AggregateTypeID;
        }

        
        
        public void Submit()
        {           
            using (var queue = new MessageQueue(@".\private$\NoSQLServer"))
            using (var message = new System.Messaging.Message())
            {
                message.Label = string.Format("{0}:{1}", ClassType.Name, aggregate.AggregateID.ToString());
                using (message.BodyStream = new MemoryStream(aggregate.ToAggregateStore().Data))
                {
                    var mqTran = new MessageQueueTransaction();
                    mqTran.Begin();
                    try
                    {
                        queue.Send(message);
                        mqTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        mqTran.Abort();
                        throw new Exception("Problem with messaging... ", ex);
                    }
                }
            }
        }
    }
}
