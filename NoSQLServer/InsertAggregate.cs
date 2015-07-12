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
    public class InsertAggregate
    {
        protected SqlCommand command = null;
        

        public long Execute(AggregateStore aggregate, string Shard)
        {
            long ID = 0;

            using (SqlConnection connection = new SqlConnection(NoSQLServerConnectionString.Value))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);


                    SqlParameter AggregateTypeID = new SqlParameter(Constants.PARAM_AGGREGATE_TYPE_ID, SqlDbType.BigInt);
                    SqlParameter Data = new SqlParameter(Constants.PARAM_DATA, SqlDbType.VarBinary, int.MaxValue);
                    SqlParameter AggregateID = new SqlParameter(Constants.PARAM_AGGREGATE_ID, SqlDbType.BigInt);
                    SqlParameter LookupValue = new SqlParameter(Constants.PARAM_LOOKUP_VALUE, SqlDbType.VarChar, 36);
                    AggregateID.Direction = ParameterDirection.Output;

                    AggregateTypeID.Value = 0;
                    Data.Value = "";

                    command.Parameters.Add(AggregateID);
                    command.Parameters.Add(AggregateTypeID);
                    command.Parameters.Add(LookupValue);
                    command.Parameters.Add(Data);
                    command.Prepare();


                    command.Parameters[Constants.PARAM_AGGREGATE_TYPE_ID].Value = aggregate.AggregateTypeID;
                    command.Parameters[Constants.PARAM_DATA].Value = aggregate.Data;
                    command.Parameters[Constants.PARAM_LOOKUP_VALUE].Value = aggregate.LookupValue;

                    command.ExecuteNonQuery();

                    ID = (long)command.Parameters[Constants.PARAM_AGGREGATE_ID].Value;
                }
            }
            return ID;
        }
    }
}
