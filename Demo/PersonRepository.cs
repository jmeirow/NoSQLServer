using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObjects.People;
using NoSQLServer;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NoSQLServer.Repository;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NoSQLServer.DBWriterService;



namespace DomainRepositories
{
    namespace People
    {
        public class PersonRepository : RepositoryBase
        {
            public PersonRepository() : base (new Person())
            {

            }

            public Person GetPersonByID (long ID)
            {
                Person p ;
                using (MemoryStream stream = readRepo.GetFetchAggregateByID().Execute(ID, Shard))
                {
                    p = (Person)formatter.Deserialize(stream);
                    p.AggregateID = ID;
                }
                return p;
            }
           

            public Person GetPersonBySSN(string SSN)
            {
                Person p = null;
                long AggregateID = 0;
                using (MemoryStream stream = readRepo.GetFetchAggregateByUniqueKey().Execute(AggregateTypeID, SSN, out AggregateID, Shard))
                {
                    p = (Person)formatter.Deserialize(stream);
                    p.AggregateID = AggregateID;
                }
                return p;
            }
        }
    }
}

