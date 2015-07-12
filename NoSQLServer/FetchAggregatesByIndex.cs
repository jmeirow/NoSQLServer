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

    public class FetchAggregatesByIndex
    {

        public List<MemoryStream> Execute(long AggregateTypeID, string ColumnNames, string DataValues, string Shard)
        {

            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {

                command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format(Constants.STORED_PROC_STRING, Shard, this.GetType().Name);


                SqlParameter pAggregateTypeID = new SqlParameter(Constants.PARAM_AGGREGATE_TYPE_ID, SqlDbType.BigInt);
                SqlParameter pColumnNames = new SqlParameter("@ColumnNames", SqlDbType.VarChar, 253);
                SqlParameter pDataValues = new SqlParameter("@DataValues", SqlDbType.VarChar, 253);

                pAggregateTypeID.Value = 0;
                command.Parameters.Add(pAggregateTypeID);
                command.Parameters.Add(pColumnNames);
                command.Parameters.Add(pDataValues);
                command.Prepare();

                List<MemoryStream> streams = new List<MemoryStream>();

                byte[] data;
                command.Parameters["@AggregateTypeID"].Value = AggregateTypeID;
                command.Parameters["@ColumnNames"].Value = ColumnNames;
                command.Parameters["@DataValues"].Value = DataValues;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data = (byte[])reader.GetValue(0);
                        streams.Add(new MemoryStream(data));
                    }
                    return streams;
                }
            }
        }
    }
}
