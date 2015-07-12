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

    public class FetchAggregateByID
    {
        protected SqlCommand command = null;

        public MemoryStream Execute(long AggregateID, string Shard)
        {

            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {
                command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);

                SqlParameter pAggregateID = new SqlParameter(Constants.PARAM_AGGREGATE_ID, SqlDbType.BigInt);
                SqlParameter pData = new SqlParameter(Constants.PARAM_DATA, SqlDbType.VarBinary, int.MaxValue);
                pData.Direction = ParameterDirection.Output;

                pAggregateID.Value = 0;
                command.Parameters.Add(pAggregateID);
                command.Parameters.Add(pData);
                command.Prepare();

                byte[] data = null;
                command.Parameters[Constants.PARAM_AGGREGATE_ID].Value = AggregateID;
                command.ExecuteNonQuery();
                data = (byte[])command.Parameters[Constants.PARAM_DATA].Value;
                MemoryStream stream = new MemoryStream(data);
                return stream;
            }
        }
    }
}
