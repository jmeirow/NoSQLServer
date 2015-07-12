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
    public class InsertAggregateType
    {
        protected SqlCommand command = null;


        //public InsertAggregateType(string Shard, SqlConnection conn)
        //{
        //    command = conn.CreateCommand();
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.CommandText = string.Format("{0}.p_{1}", Shard, this.GetType().Name);
        //    Prepare();
        //}

        //private void Prepare()
        //{
        //    SqlParameter ShortDescription = new SqlParameter("@ShortDescription", SqlDbType.VarChar, 100);
        //    SqlParameter Description = new SqlParameter("@Description", SqlDbType.VarChar, 500);
        //    SqlParameter AggregateTypeID = new SqlParameter("@AggregateTypeID", SqlDbType.BigInt);

        //    AggregateTypeID.Direction = ParameterDirection.Output;

        //    AggregateTypeID.Value = 0;
        //    Description.Value = "";
        //    ShortDescription.Value = "";

        //    command.Parameters.Add(AggregateTypeID);
        //    command.Parameters.Add(Description);
        //    command.Parameters.Add(ShortDescription);
        //    command.Prepare();
        //}

        //public long Execute(string ShortDescription, string Description)
        //{
        //    command.Parameters["@ShortDescription"].Value = ShortDescription;
        //    command.Parameters["@Description"].Value = Description;

        //    long ID = 0;

        //    command.ExecuteNonQuery();

        //    ID = (long)command.Parameters["@AggregateTypeID"].Value;

        //    return ID;
        //}
    }
}
