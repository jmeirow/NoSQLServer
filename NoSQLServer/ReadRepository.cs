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

    public class ReadRepository
    {
        public FetchAggregateByID GetFetchAggregateByID( )
        {
            return new FetchAggregateByID();
        }
        public FetchAggregateEventByID GetFetchAggregateEventByID( )
        {
            return new FetchAggregateEventByID();
        }
        public FetchAggregateEventsByAggregateID GetFetchAggregateEventsByAggregateID( )
        {
            return new FetchAggregateEventsByAggregateID();
        }


        public FetchAggregateByUniqueKey GetFetchAggregateByUniqueKey( )
        {
            return new FetchAggregateByUniqueKey();
        }
        public FetchAggregatesByIndex GetFetchAggregatesByIndex()
        {
            return new FetchAggregatesByIndex();
        }

        public FetchAggregateIDsByIndex GetFetchAggregateIDsByIndex()
        {
            return new FetchAggregateIDsByIndex();
        }
        public FetchUniqueKeyAggregateID GetFetchUniqueKeyAggregateID()
        {
            return new FetchUniqueKeyAggregateID();
        }





    }
}
