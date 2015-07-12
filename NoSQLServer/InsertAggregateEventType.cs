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

    public class InsertAggregateEventType
    {

        protected SqlCommand command = null;

        //public InsertAggregateEventType(String Shard, SqlConnection conn)
        //{
        //    command = conn.CreateCommand();
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.CommandText = string.Format("{0}.p_{1}", Shard, this.GetType().Name);
        //    Prepare();
        //}

        //private void Prepare()
        //{
        //    SqlParameter AggregateTypeID = new SqlParameter("@AggregateTypeID", SqlDbType.BigInt);
        //    SqlParameter ShortDescription = new SqlParameter("@ShortDescription", SqlDbType.Char, 100);
        //    SqlParameter Description = new SqlParameter("@Description", SqlDbType.Char, 500);
        //    SqlParameter AggregateEventTypeID = new SqlParameter("@AggregateEventTypeID", SqlDbType.BigInt);
        //    AggregateEventTypeID.Direction = ParameterDirection.Output;

        //    AggregateTypeID.Value = 0;
        //    Description.Value = "";
        //    ShortDescription.Value = "";
        //    AggregateEventTypeID.Value = 0;

        //    command.Parameters.Add(AggregateTypeID);
        //    command.Parameters.Add(Description);
        //    command.Parameters.Add(ShortDescription);
        //    command.Parameters.Add(AggregateEventTypeID);
        //    command.Prepare();
        //}

        //public long Execute(long AggregateTypeID, long AggregateEventTypeID, string Data, DateTime EventTimeStamp, string UserID)
        //{
        //    command.Parameters["@AggregateTypeID"].Value = AggregateTypeID;
        //    command.Parameters["@AggregateEventTypeID"].Value = AggregateEventTypeID;
        //    command.Parameters[Constants.PARAM_DATA].Value = Data;
        //    command.Parameters["@EventTimeStamp"].Value = EventTimeStamp;
        //    command.Parameters["@UserID"].Value = UserID;

        //    long ID = 0;

        //    using (SqlDataReader reader = command.ExecuteReader())
        //    {
        //        reader.Read();
        //        ID = reader.GetInt64(0);
        //    }
        //    return ID;
        //}
    }
}
