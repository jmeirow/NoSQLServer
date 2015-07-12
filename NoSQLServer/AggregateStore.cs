using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NoSQLServer
{

    public class AggregateStore
    {
        public long AggregateID { get; set; }
        public long AggregateTypeID { get; set; }
        public byte[] Data { get; set; }
        public int VersionNumber { get; set; }
        public string LookupValue { get; set; }
        private bool _Exists = true;
        public bool Exists
        {
            get
            {
                return _Exists;
            }
            set
            {
                _Exists = value;
            }
        }
    }
}
