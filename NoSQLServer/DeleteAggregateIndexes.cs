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

    public class DeleteAggregateIndexes
    {

        protected SqlCommand command = null;


 

        public void Execute(AggregateStore aggregate, string Shard)
        {

            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {
 
                command.Connection.Open();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);

                SqlParameter AggregateTypeID = new SqlParameter(Constants.PARAM_AGGREGATE_TYPE_ID, SqlDbType.BigInt);
                SqlParameter AggregateID = new SqlParameter(Constants.PARAM_AGGREGATE_ID, SqlDbType.BigInt);


                AggregateTypeID.Value = 0;
                AggregateTypeID.Value = 0;

                command.Parameters.Add(AggregateID);
                command.Parameters.Add(AggregateTypeID);

                command.Parameters[Constants.PARAM_AGGREGATE_ID].Value = aggregate.AggregateID;
                command.Parameters[Constants.PARAM_AGGREGATE_TYPE_ID].Value = aggregate.AggregateTypeID;
                command.ExecuteNonQuery();

            }
        }
    }
}
