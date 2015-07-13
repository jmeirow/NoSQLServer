
#NoSQLServer

In an era where infrastructure costs continue to slowly fall, the ongoing shortage of qualified developers means that development costs continue to be, by far, the largest cost of brining a new system into existence. Certainly NoSQL databases and their "schemaless" approach to storage have been a tremendous boon to developer productivity in recent years. Still, bringing a NoSQL database system into an environment is not without its initial and ongoing costs. Beyond the initial costs, there is the added cost of potentially adding a person to administer the new system or at the very least, heavily training an existing DBA. 

Beyond the costs, there is the additional challenge of building a single application utlizing both storage mechanisms. Your RDBMS data isn't going anywhere fast and your existing software is written to your relational data. New systems and new enhancements to existing systems can be written to use the legacy data where applicable and to use document-storage in new code, but what happens if there is a failure of the sort that normally requires a restore to a last complete backup and a rolling-forward of the transaction logs? With two disparate database engines supporting a single application, that sort of fine-grained restore is all but impossible. Sure, you *can* configure both engines to utilize the same storage and do a restore at the file level, but that won't yield the same sort of fine-grained control as a restore/roll-forward. 

NoSQLServer was desgined with small shops in mind, the kind of shops that have limited DBA resources and limited development staff. NoSQLServer is a collection of .NET assemblies that your code can leverage to store objects to the database without the need for ORMs, new tables or new procedures. Using only three SQL Server tables, NoSQLServer can store an infinite number of object types. It can enforce unique constraints and you can create indexes on data within "the BLOB" for performance. You EASILY add and remove properties from classes and rename properties when necessary. Most impressively, you can cache collections of objects inside other objects as "view models" for performance purposes and NoSQLServer will automatically (and asynchronously) update those cached objects when the authoritative object is updated.  All of this obtainable with C# classes alone: no new stored procedures, no new tables. 

ASP.NET MVC view "wizards" work with NoSQLServer-derived classes out of the box, so there's no productivity loss when writing MVC web apps. The NoSQLServer Client project, which will be posted soon, provides ad-hoc query capabilities to your NoSQLServer objects. The NoSQLServer client functions very much like the query tool SQL Management Studio, except that you write your queries, updates, inserts and deletes in C#/LINQ expressions. Any manipulations done through the NoSQLServer client cascade to cached objects as well, provided the cascading update is registed in the Cascading Update Registry. Think of a cascading update as essentially like a SQL-trigger, except that it operates asynchronously and is written in C# or an CLR language.



##Getting Started

NoSQLServer gives you a powerful, object-based way to build dynamic applications that enables a clean separation of concerns and gives you full control object composition without the need for tables and stored procedures for enjoyable, agile development and easy change-mangement/deployment.

##Easier Development

Entity Framework? Forget about it! Your business logic or controllers can directly read/write view models providing increased performance with decreased code! Get rid of those mappings and the havoc wreaked upon your builds when a table changes. Best of all, the ASP.NET MVC view wizards work with NoSQLServer objects out of the box.


##Easier Change Management

No "CREATE/ALTER TABLE" scripts and no stored procedures mean simplified change management because there are far fewer code objects to develop, maintain and deploy.

##100% Microsoft Technology

* Data is stored in Microsoft SQL Server<br/>
* All data access is via stored procedures<br/>
* Write your classes in C# or any MS-CLR language<br/>


#### Topics

*[Your First Class](#Your First Class)<br/>
*[Where Does the Data Go?](#Where Does the Data Go?)<br/>
*[Registering Your Class](#Registering Your Class)<br/>
*[Your First Repository](#Your First Repository)<br/>
*[Putting It All Together](#Putting It All Together)<br/>
*[Adding a Field to an Existing Class](#Adding a Field to an Existing Class)<br/>
*[Deleting a Field From an Existing Class](#Deleting a F)<br/>
*[Changing the Name of a Field on an Existing Class](#Changing the Name of an Existing Field)<br/>
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

```C#

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQLServer;                                                  // <-- reference to NoSQLServer


namespace DomainObjects.People
{
    [Serializable]                                                 // <-- Serializable
    public class Person : AggregateBase                            // <-- Inherit from AggregateBase
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
           // removed for brevity, see source code in Demo project.

        }
        public override string ToString()
        {
            // removed for brevity, see source code in Demo project.
        }


        public override string GetUniqueKey()                               // <-- required by baseclass
        {
            return SSN;
        }

        public override Dictionary<string, string> GetIndexes()            // <-- required by baseclass
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

```






####<a name="Where Does the Data Go?">Where Does the Data Go?</a><br/>
####<a name="Registering Your Class">Registering Your Class</a><br/>
####<a name="Your First Repository">Your First Repository</a><br/>
####<a name="Putting It All Together">Putting It All Together</a><br/>
####<a name="Adding a Field to an Existing Class">Adding a Field to an Existing Class</a><br/>
####<a name="Deleting a Field From an Existing Class">Deleting a Field From a Class</a><br/>
####<a name="Changing the Name of a Field on an Existing Class">Changing the Name of an Existing Field</a><br/>
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





