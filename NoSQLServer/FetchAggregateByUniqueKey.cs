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
    public class FetchAggregateByUniqueKey
    {


 
        public MemoryStream Execute(long AggregateTypeID, string KeyValue, out long AggregateID, string Shard)
        {
            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {
                command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);


                SqlParameter pAggregateTypeID = new SqlParameter(Constants.PARAM_AGGREGATE_TYPE_ID, SqlDbType.BigInt);
                SqlParameter pLookupValue = new SqlParameter(Constants.PARAM_LOOKUP_VALUE, SqlDbType.VarChar, 36);
                SqlParameter pData = new SqlParameter(Constants.PARAM_DATA, SqlDbType.VarBinary, int.MaxValue);
                SqlParameter pAggregateID = new SqlParameter(Constants.PARAM_AGGREGATE_ID, SqlDbType.BigInt);
                pAggregateID.Direction = ParameterDirection.Output;
                pData.Direction = ParameterDirection.Output;

                pAggregateTypeID.Value = 0;
                command.Parameters.Add(pAggregateTypeID);
                command.Parameters.Add(pAggregateID);
                command.Parameters.Add(pLookupValue);
                command.Parameters.Add(pData);
                command.Prepare();

                byte[] data;
                command.Parameters[Constants.PARAM_AGGREGATE_TYPE_ID].Value = AggregateTypeID;
                command.Parameters[Constants.PARAM_AGGREGATE_ID].Value = 0;
                command.Parameters[Constants.PARAM_LOOKUP_VALUE].Value = KeyValue;
                command.ExecuteNonQuery();
                data = (byte[])command.Parameters[Constants.PARAM_DATA].Value;
                MemoryStream stream = new MemoryStream(data);
                AggregateID = (long)command.Parameters[Constants.PARAM_AGGREGATE_ID].Value;
                return stream;
            }
        }
    }
}
