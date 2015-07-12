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

    public class InsertAggregateEvent
    {
        protected SqlCommand command = null;


        //public InsertAggregateEvent(string Shard, SqlConnection conn)
        //{
        //    command = conn.CreateCommand();
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.CommandText = string.Format("{0}.p_{1}", Shard, this.GetType().Name);
        //    Prepare();
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

        //private void Prepare()
        //{

        //    SqlParameter AggregateTypeID = new SqlParameter("@AggregateTypeID", SqlDbType.BigInt);
        //    SqlParameter AggregateEventTypeID = new SqlParameter("@AggregateEventTypeID", SqlDbType.BigInt);
        //    SqlParameter Data = new SqlParameter(Constants.PARAM_DATA, SqlDbType.VarBinary, int.MaxValue);
        //    SqlParameter EventTimeStamp = new SqlParameter("@EventTimeStamp", SqlDbType.DateTime);
        //    SqlParameter UserID = new SqlParameter("@UserID", SqlDbType.VarChar, 30);
        //    SqlParameter AggregateEventID = new SqlParameter("@AggregateEventID", SqlDbType.BigInt);
        //    AggregateEventID.Direction = ParameterDirection.Output;


        //    AggregateTypeID.Value = 0;
        //    AggregateEventTypeID.Value = 0;
        //    Data.Value = "";
        //    EventTimeStamp.Value = DBNull.Value;
        //    UserID.Value = "";


        //    command.Parameters.Add(AggregateTypeID);
        //    command.Parameters.Add(AggregateEventTypeID);
        //    command.Parameters.Add(Data);
        //    command.Parameters.Add(EventTimeStamp);
        //    command.Parameters.Add(UserID);
        //    command.Parameters.Add(AggregateEventID);
        //    command.Prepare();
        //}
    }
}
