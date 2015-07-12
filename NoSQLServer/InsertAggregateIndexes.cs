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


    public class InsertAggregateIndexes
    {
        public void Execute(AggregateStore aggregate, string ColumnNames, string DataValues, string Shard)
        {
            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {
                command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);

                SqlParameter pAggregateTypeID = new SqlParameter(Constants.PARAM_AGGREGATE_TYPE_ID, SqlDbType.BigInt);
                SqlParameter pDataValues = new SqlParameter(Constants.PARAM_DATA_VALUES, SqlDbType.VarChar, 40);
                SqlParameter pAggregateID = new SqlParameter(Constants.PARAM_AGGREGATE_ID, SqlDbType.BigInt);
                SqlParameter pColumnNames = new SqlParameter(Constants.PARAM_COLUMN_NAMES, SqlDbType.VarChar, 40);


                command.Parameters.Add(pAggregateID);
                command.Parameters.Add(pAggregateTypeID);
                command.Parameters.Add(pDataValues);
                command.Parameters.Add(pColumnNames);
                command.Prepare();


                command.Parameters[Constants.PARAM_AGGREGATE_TYPE_ID].Value = aggregate.AggregateTypeID;
                command.Parameters[Constants.PARAM_AGGREGATE_ID].Value = aggregate.AggregateID;
                command.Parameters[Constants.PARAM_COLUMN_NAMES].Value = ColumnNames;
                command.Parameters[Constants.PARAM_DATA_VALUES].Value = DataValues;
                command.ExecuteNonQuery();
            }
        }
    }
}
