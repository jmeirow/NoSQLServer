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

    public class FetchAggregateEventsByAggregateID
    {

        protected SqlCommand command = null;


 

 
        public void Execute(string Shard)
        {

            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {
                command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);


                SqlParameter AggregateID = new SqlParameter(Constants.PARAM_AGGREGATE_ID, SqlDbType.BigInt);
                AggregateID.Value = 0;
                command.Parameters.Add(AggregateID);
                command.Prepare();
            }
        }
    }
}
