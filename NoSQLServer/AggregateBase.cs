using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQLServer;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NoSQLServer
{
    [Serializable]
    public abstract class AggregateBase : IAggregateBase
    {
        public AggregateStore ToAggregateStore()
        {
            AggregateStore obj = new AggregateStore();
            obj.AggregateTypeID = AggregateTypeID;
            obj.AggregateID = AggregateID;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                obj.Data = stream.ToArray();
            }
            obj.LookupValue = GetUniqueKey();
            return obj;
        }

        public AggregateBase()
        {

        }
        public virtual string GetUniqueKey()
        {
            return Guid.NewGuid().ToString();
        }

        public bool HasIndexes()
        {
            return GetIndexes().Count > 0;
        }

        public abstract Dictionary<string, string> GetIndexes();

 


        private long _AggregateID ;
        public long AggregateID
        {
            get 
            {
                return  _AggregateID;
            }
            set
            {
                _AggregateID = value;
            }
        }
        private long _AggregateTypeID ;
        public long AggregateTypeID
        {
            get 
            {
                return  _AggregateTypeID;
            }
            set
            {
                _AggregateTypeID = value;
            }
        }
    
        private int _VersionNumber ;
        public int VersionNumber
        {
            get 
            {
                return  _VersionNumber;
            }
            set
            {
                _VersionNumber = value;
            }
        }
    }
}
