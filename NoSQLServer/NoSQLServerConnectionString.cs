using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;




namespace NoSQLServer
{
    public class NoSQLServerConnectionString
    {
        public static string Value { get; private set; }
        static NoSQLServerConnectionString()
        {
            Value = System.Configuration.ConfigurationManager.ConnectionStrings["NoSQLServer"].ConnectionString;
        }
    }
}
