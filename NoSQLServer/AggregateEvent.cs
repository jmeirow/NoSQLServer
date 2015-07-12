using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQLServer
{
    public class AggregateEvent
    {
        public long AggregateEventID { get; set; }
        public long AggregateEventTypeID { get; set; }
        public long AggregateID { get; set; }
        public byte[] Data { get; set; }
        public DateTime EventTimeStamp { get; set; }
        public string UserID { get; set; }
    }

}
