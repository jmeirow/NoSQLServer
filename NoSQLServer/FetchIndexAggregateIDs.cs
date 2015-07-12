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

    public class FetchAggregateIDsByIndex
    {

        public long Execute(long AggregateTypeID, string ColumnNames, string DataValues, string Shard)
        {

            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {
                command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard.Trim(), this.GetType().Name);


                SqlParameter pAggregateTypeID = new SqlParameter(Constants.PARAM_AGGREGATE_TYPE_ID, SqlDbType.BigInt);
                SqlParameter pColumnNames = new SqlParameter(Constants.PARAM_COLUMN_NAMES, SqlDbType.VarChar, 253);
                SqlParameter pDataValues = new SqlParameter(Constants.PARAM_DATA_VALUES, SqlDbType.VarChar, 253);
                SqlParameter pAggregateID = new SqlParameter(Constants.PARAM_AGGREGATE_ID, SqlDbType.BigInt);
                pAggregateID.Direction = ParameterDirection.Output;

                pAggregateTypeID.Value = 0;
                command.Parameters.Add(pAggregateTypeID);
                command.Parameters.Add(pColumnNames);
                command.Parameters.Add(pDataValues);
                command.Parameters.Add(pAggregateID);
                command.Prepare();


                command.Parameters[Constants.PARAM_AGGREGATE_TYPE_ID].Value = AggregateTypeID;
                command.Parameters[Constants.PARAM_COLUMN_NAMES].Value = ColumnNames;
                command.Parameters[Constants.PARAM_DATA_VALUES].Value = DataValues;
                command.ExecuteNonQuery();

                return (command.Parameters[Constants.PARAM_AGGREGATE_ID].Value == DBNull.Value ? 0 : (long)command.Parameters[Constants.PARAM_AGGREGATE_ID].Value);
            }
        }
    }

}
