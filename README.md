
#NoSQLServer

NoSQLServer is a .NET library for building great .NET applications using a class-centric approach to modeling and implementing your domain. NoSQLServer gives you the ease of NoSQL-style programming while recognizing that bringing a new technology platform into your shop is not always a viable option. NoSQLServer will run (with slight modifications) on virtually any RDBMS. 

##Getting Started

NoSQLServer gives you a powerful, object-based way to build dynamic applicaitons that enables a clean separation of concerns and gives you full control object composition without the need for tables and stored procedures for enjoyable, agile development and easy change-mangement/deployment.

##Easier Development

Entity Framework? Forget about it! Your business logic or controllers can directly read/write view models providing increased performance with decreased code! Get rid of those mappings and the havoc wreaked upon your builds when a table changes. Best of all, the ASP.NET MVC view wizards work with NoSQLServer objects out of the box.


##Easier Change Management

No "CREATE/ALTER TABLE" scripts and no stored procedures mean simplified change management because there are far fewer code objects to develop, maintain and deploy.

##100% Microsoft Technology

*Data is stored in Microsoft SQL Server 
*All data access is via stored procedures
*Write your classes in C# or any MS-CLR language


#### Topics

*[Your First Class](#Your First Class)<br/>
*[Where Does the Data Go?](#Where Does the Data Go?)<br/>
*[Registering Your Class](#Registering Your Class)<br/>
*[Your First Repository](#Your First Repository)<br/>
*[Putting It All Together](#Putting It All Together)<br/>
*[Adding a Field to an Existing Class](#Adding a Field to an Existing Class)<br/>
*[Deleting a F](#Deleting a F)<br/>
*[Changing the Name of an Existing Field](#Changing the Name of an Existing Field)<br/>
*[What Can't I (Easily) Change?](#What Can't I (Easily) Change?)<br/>
*[Singleton Objects and Lookup Classes](#Singleton Objects and Lookup Classes)<br/>
*[The AggregateTypeInfo Object](#The AggregateTypeInfo Object)<br/>
*[Nested Objects](#Nested Objects)<br/>
*[Cascading Updates](#Cascading Updates)<br/>
*[Understanding Unique Keys](#Understanding Unique Keys)<br/>
*[Understanding Indexes](#Understanding Indexes)<br/>
*[Creating Indexes](#Creating Indexes)<br/>
*[Understanding Sharding](#Understanding Sharding)<br/>
*[Creating a New Shard](#Creating a New Shard)<br/>
*[Commands and Invokers](#Commands and Invokers)<br/>
*[Single Writer Pattern](#Single Writer Pattern)<br/>
*[Unit of Work Pattern](#Unit of Work Pattern)<br/>
*[Aggregate Events](#Aggregate Events)<br/>
*[Replaying Events in the Debugger](#Replaying Events in the Debugger)<br/>
*[Maintaining a BI Database From NoSQLServer](#Maintaining a BI Database From NoSQLServer)<br/>
*[Assemblies and References](#Assemblies and References)<br/>
*[Browsing Object Data (like in Query Analyzer)](#Browsing Object Data (like in Query Analyzer))<br/>
*[The Classes in This Demo App](#The Classes in This Demo App)<br/>
 



####<a name="Your First Class">Your First Class</a><br/>

...

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQLServer;


namespace DomainObjects.People
{
    [Serializable]
    public class Person : AggregateBase
    {

        private string _FirstName;
        public string FirstName
        {
            get
            {

                return _FirstName;
            }
            set
            {

                _FirstName = value;
            }
        }

        private string _LastName;
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
            }
        }



        private DateTime _BirthDate;
        public DateTime BirthDate
        {
            get
            {
                return _BirthDate;
            }
            set
            {
                _BirthDate = value;
            }
        }



        private string _Gender;
        public string Gender
        {
            get
            {
                return _Gender;
            }
            set
            {
                _Gender = value;
            }
        }



        private string _SSN;
        public string SSN
        {
            get
            {
                return _SSN;
            }
            set
            {
                _SSN = value;
            }
        }


        public static string ToStringHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('\r').Append('\n');
            sb.Append(string.Format(" {0,20}   {1,20}   {2,10}     {3,20}    {4,9}   {5,20}   {6,20}", "First Name", "Last Name", "Gender", "Birth Date", "SSN", "AggregateID", "AggregateTypeID"));
            sb.Append('\r').Append('\n');
            sb.Append(string.Format(" {0,20}   {1,20}   {2,10}     {3,20}    {4,9}   {5,20}   {6,20}", "==========", "===========", "=========", "==========", "========", "===========", "================"));
            return sb.ToString();

        }
        public override string ToString()
        {
            return string.Format(" {0,20}   {1,20}   {2,10}     {3,20}    {4,9}  {5,20}   {6,20}", FirstName, LastName, Gender, BirthDate.ToShortDateString(), SSN, AggregateID, AggregateTypeID);
        }


        public override string GetUniqueKey()
        {
            return SSN;
        }

        public override Dictionary<string, string> GetIndexes()
        {
            Dictionary<string, string> indexes = new Dictionary<string, string>();
            IndexData indexData = new IndexData();
            indexData.Add("LastName", LastName);
            indexData.Compute();
            indexes.Add(indexData.ColumnNames, indexData.DataValues);
            return indexes;
        }
    }
}

...






####<a name="Where Does the Data Go?">Where Does the Data Go?</a><br/>
####<a name="Registering Your Class">Registering Your Class</a><br/>
####<a name="Your First Repository">Your First Repository</a><br/>
####<a name="Putting It All Together">Putting It All Together</a><br/>
####<a name="Adding a Field to an Existing Class">Adding a Field to an Existing Class</a><br/>
####<a name="Deleting a Field From a Class">Deleting a Field From a Class</a><br/>
####<a name="Changing the Name of an Existing Field">Changing the Name of an Existing Field</a><br/>
####<a name="What Can't I (Easily) Change?">What Can't I (Easily) Change?</a><br/>
####<a name="Singleton Objects and Lookup Classes">Singleton Objects and Lookup Classes</a><br/>
####<a name="The AggregateTypeInfo Object">The AggregateTypeInfo Object</a><br/>
####<a name="Nested Objects">Nested Objects</a><br/>
####<a name="Cascading Updates">Cascading Updates</a><br/>
####<a name="Understanding Unique Keys">Understanding Unique Keys</a><br/>
####<a name="Understanding Indexes">Understanding Indexes</a><br/>
####<a name="Creating Indexes">Creating Indexes</a><br/>
####<a name="Understanding Sharding">Understanding Sharding</a><br/>
####<a name="Creating a New Shard">Creating a New Shard</a><br/>
####<a name="Commands and Invokers">Commands and Invokers</a><br/>
####<a name="Single Writer Pattern">Single Writer Pattern</a><br/>
####<a name="Unit of Work Pattern">Unit of Work Pattern</a><br/>
####<a name="Aggregate Events">Aggregate Events</a><br/>
####<a name="Replaying Events in the Debugger">Replaying Events in the Debugger</a><br/>
####<a name="Maintaining a BI Database From NoSQLServer">Maintaining a BI Database From NoSQLServer</a><br/>
####<a name="Assemblies and References](#Assemblies">Assemblies and References](#Assemblies</a><br/>
####<a name="Browsing Object Data (like in Query Analyzer)">Browsing Object Data (like in Query Analyzer)</a><br/>
####<a name="The Classes in This Demo App">The Classes in This Demo App</a><br/>





