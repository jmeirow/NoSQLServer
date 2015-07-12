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
    public class UpdateAggregate
    {
        public void Execute(AggregateStore store, string LookupValue, string Shard)
        {
            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {
                command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);

                SqlParameter pData = new SqlParameter(Constants.PARAM_DATA, SqlDbType.VarBinary, int.MaxValue);
                SqlParameter pAggregateID = new SqlParameter(Constants.PARAM_AGGREGATE_ID, SqlDbType.BigInt);
                SqlParameter pLookupValue = new SqlParameter(Constants.PARAM_LOOKUP_VALUE, SqlDbType.VarChar, 36);
                pAggregateID.Value = 0;
                pData.Value = "";
                pLookupValue.Value = "";

                command.Parameters.Add(pLookupValue);
                command.Parameters.Add(pAggregateID);
                command.Parameters.Add(pData);
                command.Parameters[Constants.PARAM_AGGREGATE_ID].Value = store.AggregateID;
                command.Parameters[Constants.PARAM_DATA].Value = store.Data;
                command.Parameters[Constants.PARAM_LOOKUP_VALUE].Value = LookupValue;
                command.ExecuteNonQuery();
            }
        }
    }
}
