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
    public abstract class RepositoryMethod
    {
        protected SqlConnection conn;


        public RepositoryMethod(String Shard, SqlConnection conn)
        {

        }
    }

}
