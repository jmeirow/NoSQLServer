using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace NoSQLServer
{

    public class FetchAggregateEventByID
    {
        protected SqlCommand command = null;


        public string Execeute(long AggregateEventID, string Shard)
        {
            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {
                command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);

                command.CommandText = @"p_GetAggregateEventByID";
                SqlParameter pAggregateEventID = new SqlParameter("@AggregateEventID", SqlDbType.BigInt);
                pAggregateEventID.Value = 0;
                command.Parameters.Add(pAggregateEventID);
                command.Prepare();



                command.Parameters["@AggregateEventID"].Value = AggregateEventID;
                string data = string.Empty;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    data = reader.GetString(0);
                }
                
                return data;
            }
        }
    }
}
