using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQLServer;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.Data.SqlClient;
using System.Messaging;



namespace NoSQLServer.DBWriterService
{
    public class QueueWriter
    {
        AggregateBase aggregate;
        AggregateStore store;
        string Shard;

        protected BinaryFormatter formatter = new BinaryFormatter();

        public QueueWriter(AggregateBase aggregate, AggregateStore store, string Shard)
        {
            this.aggregate = aggregate;
            this.store = store;
            this.Shard = Shard;
        }
        public void Send()
        {
            MessageQueue queue = new MessageQueue(@".\Private$\NoSQLServer_DomainUpdater");
            Message message = new Message();
            message.Recoverable = true;
            message.Label = string.Format("{0}:{1}:{2}", aggregate.AggregateID, aggregate.GetType().FullName, Shard);
            queue.Send(message);
        }
    }







}
