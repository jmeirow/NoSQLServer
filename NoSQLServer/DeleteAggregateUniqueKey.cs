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
    public class DeleteAggregate
    {
        public void Execute(long AggregateID, string Shard)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.Connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);


                    SqlParameter pAggregateID = new SqlParameter(Constants.PARAM_AGGREGATE_ID, SqlDbType.BigInt);
                    command.Parameters.Add(pAggregateID);
                    command.Prepare();
                    command.Parameters[Constants.PARAM_AGGREGATE_ID].Value = AggregateID;

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlex)
            {
                throw new Exception("Delete failed!", sqlex);
            }
        }
    }
}
