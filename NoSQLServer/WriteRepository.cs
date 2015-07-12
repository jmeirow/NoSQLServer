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
    public class WriteRepository
    {


        public WriteRepository()
        {
        }

        public InsertAggregate GetInsertAggregate()
        {
            return new InsertAggregate();
        }

        public UpdateAggregate GetUpdateAggregate( )
        {
            return new UpdateAggregate();
        }

        public InsertAggregateType GetInsertAggregateType( )
        {
            return new InsertAggregateType();
        }

        public InsertAggregateEvent GetInsertAggregateEvent( )
        {
            return new InsertAggregateEvent();
        }

        public InsertAggregateEventType GetInsertAggregateEventType( )
        {
            return new InsertAggregateEventType();
        }

        public InsertAggregateIndexes GetInsertAggregateIndex( )
        {
            return new InsertAggregateIndexes();
        }

        public DeleteAggregate GetDeleteAggregate( )
        {
            return new DeleteAggregate();
        }

        public DeleteAggregateIndexes GetDeleteAggregateIndexes()
        {
            return new DeleteAggregateIndexes();
        }
        

    }
}
