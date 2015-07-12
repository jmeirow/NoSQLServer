
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
                 

[Your First Class](#Your First Class)
[Where Does the Data Go?](#Where Does the Data Go?)
[Registering Your Class](#Registering Your Class)
[Your First Repository](#Your First Repository)
[Putting It All Together](#Putting It All Together)
[Adding a Field to an Existing Class](#Adding a Field to an Existing Class)
[Deleting a F](#Deleting a F)
[Changing the Name of an Existing Field](#Changing the Name of an Existing Field)
[What Can't I (Easily) Change?](#What Can't I (Easily) Change?)
[Singleton Objects and Lookup Classes](#Singleton Objects and Lookup Classes)
[The AggregateTypeInfo Object](#The AggregateTypeInfo Object)
[Nested Objects](#Nested Objects)
[Cascading Updates](#Cascading Updates)
[Understanding Unique Keys](#Understanding Unique Keys)
[Understanding Indexes](#Understanding Indexes)
[Creating Indexes](#Creating Indexes)
[Understanding Sharding](#Understanding Sharding)
[Creating a New Shard](#Creating a New Shard)
[Commands and Invokers](#Commands and Invokers)
[Single Writer Pattern](#Single Writer Pattern)
[Unit of Work Pattern](#Unit of Work Pattern)
[Aggregate Events](#Aggregate Events)
[Replaying Events in the Debugger](#Replaying Events in the Debugger)
[Maintaining a BI Database From NoSQLServer](#Maintaining a BI Database From NoSQLServer)
[Assemblies and References](#Assemblies and References)
[Browsing Object Data (like in Query Analyzer)](#Browsing Object Data (like in Query Analyzer))
[The Classes in This Demo App](#The Classes in This Demo App)
[Help Wanted](#Help Wanted)



 
<a name="yourfirstclass">Your First Class</a>






