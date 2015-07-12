using System;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using NoSQLServer;
using DomainRepositories.People;
using DomainObjects.People;


namespace NoSQLServer
{
    public class Demo
    {
        static void Main(string[] args)
        {

            Person p = new Person();
            PersonRepository repo = new PersonRepository();

            p.FirstName = "John";
            p.LastName = "Doe";
            p.Gender = "M";
            p.BirthDate = new DateTime(1987,6,1);
            p.SSN = "123456789";
            long ID = repo.Save(p);

            Console.WriteLine("Original saved object.");
            Console.WriteLine(Person.ToStringHeader());
            Console.WriteLine(p.ToString());
            Console.WriteLine("\r\n\r\n");


            Console.WriteLine("Person retreived from DB by SSN.");
            Person z = repo.GetPersonBySSN(p.SSN);
            Console.WriteLine(Person.ToStringHeader());
            Console.WriteLine(z.ToString());



            Console.ReadLine();

        }
    }
}
