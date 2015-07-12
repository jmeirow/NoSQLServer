using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;







namespace NoSQLServer
{

    [Serializable]

    public class AggregateInfo
    {
        public long AggregateID;
        public long AggregateTypeID;
        public int VersionNumber;
        public string LookupValue;
        public Type ClassType;
        public static readonly string fetch = "select FullyQualifiedTypeName from  NoSQLServer_MASTER.AggregateTypes where AggregateTypeID = {0}";
        
        public AggregateInfo(AggregateStore store)
        {
            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(fetch, AggregateTypeID);
                    this.AggregateID = store.AggregateID;
                    this.AggregateTypeID = store.AggregateTypeID;
                    this.VersionNumber = store.VersionNumber;
                    this.LookupValue = store.LookupValue;
                }
           }
        }
    }
}
