using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;



namespace NoSQLServer
{
    public class FetchCascadingUpdates
    {
        public List<String> Execute(long AggregateTypeID)
        {
            List<string> results = new List<String>();
            using (SqlConnection conn = new SqlConnection(NoSQLServerConnectionString.Value))
            using (SqlCommand command = conn.CreateCommand())
            {

                command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText =  "NoSQLServer_MASTER.p_FetchCascadingUpdates" ;


                SqlParameter pAggregateTypeID = new SqlParameter("@AggregateTypeID", SqlDbType.BigInt);
                command.Parameters.Add(pAggregateTypeID);
                pAggregateTypeID.Value = 0;
                command.Prepare();


                command.Parameters["@AggregateTypeID"].Value = AggregateTypeID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(reader.GetString(0));
                    }
                    return results;
                }
            }
        }
    }
}
