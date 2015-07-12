using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;



namespace NoSQLServer
{
    public class AggregateTypeInfo
    {
        public AggregateTypeInfo( long ID, string Shard, string Description)
        {
            this.ID = ID;
            this.Shard = Shard;
            this.Description = Description;
        }
        public AggregateTypeInfo() { }

        public long ID;
        public string Shard;
        public string Description;
    }



    public class AggregateType
    {
        public long AggregateTypeID { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public static AggregateTypeInfo GetInfo(string name)
        {
            AggregateTypeInfo info = new AggregateTypeInfo();

            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {
                command.Connection.Open();
                command.CommandText = string.Format("select AggregateTypeID, Shard, Description from NoSQLServer_MASTER.AggregateTypes where FullyQualifiedTypeName = '{0}'", name);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    info = new AggregateTypeInfo(reader.GetInt64(reader.GetOrdinal("AggregateTypeID")),
                                                        reader.GetString(reader.GetOrdinal("Shard")).Trim(),
                                                        reader.GetString(reader.GetOrdinal("Description")).Trim()
                                                    );
                    return info;
                }
            }
        }
    }
}
