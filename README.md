<div class="jumbotron">

# NoSQLServer

### Online Documentation

</div>

<div class="col-md-12">

### Getting started

NoSQLServer gives you a powerful, object-based way to build dynamic applicaitons that enables a clean separation of concerns and gives you full control object composition without the need for tables and stored procedures for enjoyable, agile development and easy change-mangement/deployment.

### Creating a Simple Class

Let's start by creating a "Person" class. When we're done, we'll be able to persist a Person object to the database, retrieve multiple ways, update it save changes to it, cache it inside of _other_ objects and delete...all without creating a single stored procedure and even without creating a table!

##### [Your First Class](#1)

##### [Where Does the Data Go?](#2)

##### [Registering Your Class](#3)

##### [Your First Repository](#4)

##### [Putting It All Together](#5)

##### [Adding a Field to an Existing Class](#6)

##### [Deleting a Field](#7)

[](#7)

##### [](#7)[Changing the Name of an Existing Field](#8)

##### [What Can't I (Easily) Change?](#9)

##### [Singleton Objects and Lookup Classes](#10)

##### [The AggregateTypeInfo Object](#11)

##### [Nested Objects](#12)

##### [Cascading Updates](#13)

##### [Understanding Unique Keys](#14)

##### [Understanding Indexes](#15)

##### [Creating Indexes](#16)

##### [Understanding Sharding](#17)

##### [Creating a New Shard](#18)

##### [Commands and Invokers](#19)

##### [Single Writer Pattern](#27)

##### [Unit of Work Pattern](#28)

##### [Aggregate Events](#20)

##### [Replaying Events in the Debugger](#21)

##### [Maintaining a BI Database From NoSQLServer](#22)

##### [Assemblies and References](#24)

##### [Browsing Object Data (like in Query Analyzer)](#25)

##### [The Classes in This Demo App](#26)

##### [Help Wanted](#23)

#### Your First Class

To create your first NoSQLServer class, you can take a normal POCO (plain old C# object) with public properties and make only four changes:

1.  Make it Serailizable
2.  Make it inherit from AggregagteBase (which means you'll need to make a reference to "NoSQLServer")
3.  Add a method called GetUniqueKey that returns string and "overrides" the virtual method in AggregateBase.
4.  Add a method called GetIndexes that returns generic dictionary of <string,string> and "overrides" the method in AggregateBase.

See the highlighted areas of code below that demonstrate the implementation of the rules above.

**Lastly**, your properties must use a private backing variable. NoSQLServer uses _binary_ serialization to store your objects in the database and binary serialization does not work with automatic properties.

<pre>    `using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NoSQLServer;
    namespace DomainObjects
    {
        namespace Enrollment
        {

    <span style="background-color:lightyellow">[Serializable]</span> 
            public class Person  <span style="background-color:lightyellow">  :  AggregateBase </span> 
            {

                private string _FirstName ;
                public string FirstName
                {
                    get 
                    {
                        return  _FirstName;
                    }
                    set
                    {
                        _FirstName = value;
                    }
                }
                private string _LastName ;
                public string LastName
                {
                    get 
                    {
                        return  _LastName;
                    }
                    set
                    {
                        _LastName = value;
                    }
                }
                private string _MiddleName;
                public string MiddleName
                {
                    get
                    {
                        return _MiddleName ?? string.Empty;
                    }
                    set
                    {
                        _MiddleName = value;
                    }
                }
                private string _Gender ;
                public string Gender
                {
                    get 
                    {
                        return  _Gender;
                    }
                    set
                    {
                        _Gender = value;
                    }
                }

                private DateTime _BirthDate ;
                public DateTime BirthDate
                {
                    get 
                    {
                        return  _BirthDate;
                    }
                    set
                    {
                        _BirthDate = value;
                    }
                }
                private string _SSN ;
                public string SSN
                {
                    get 
                    {
                        return  _SSN;
                    }
                    set
                    {
                        _SSN = value;
                    }
                }

                public override string GetUniqueKey()
                {
                    return SSN;
                }

                public override Dictionary<string>GetIndexes()
                {
                    return   new Dictionary<string, string>();
                }

            }
        }
    }` 
    </pre>

#### Where Does the Data Go?

All of your object's data is stored in the Aggregates table, specifically the _Data_ column which is defined as VARBINARY(MAX). VARBINARY(MAX) can hold 2GB of space. Typical objects will consume under 1K of data but some view models or nested objects could be larger. Clearly 2GB can hold A LOT of serialized data.

        CREATE TABLE NoSQLServer_01.Aggregates (
    	    AggregateID      BIGINT NOT NULL IDENTITY PRIMARY KEY ,
    	    AggregateTypeID  BIGINT NOT NULL ,
    	    Data             VARBINARY(MAX),
    	    VersionNumber    INT,
    	    ObjectTimestamp  DATETIME NOT NULL DEFAULT GETDATE(),
    	    LookupValue      VARCHAR(36) NOT NULL    
        )

#### Registering Your Class

NoSQLServer actually stores your data in SQLServer tables (but not very many of them). One of the two most important tables in the system is _AggregateTypes_ which is defined in a schema named _NoSQLServer_MASTER_. You'll need to insert a record into this table for your type. For our Person class we've just created we can use the following INSERT statement:

        insert into NoSQLServer_MASTER.AggregateTypes  
        (
            FullyQualifiedTypeName,
            Description,
            Shard
        ) 
            values 
        (
            'DomainObjects.Enrollment.Person',
            'This collection of all "people" in the system.',
            'NoSQLServer_01'
        )

The columns of this table are:  

<dl>

<dt>AggregateTypeID</dt>

<dd>Identity column. The number assigned is what the system uses to identify this type.</dd>

<dt>FullyQualifiedTypeName</dt>

<dd>This is the full namespace plus class name. In our exampe it is "DomainObject.Enrollment.Person"</dd>

<dt>Description</dt>

<dd>This is a free-form description field where we can describe in detail what this data is for. No need for crazy long class names because we can put the crazy long descrition here and read it whenever we want.</dd>

<dt>Shard</dt>

<dd>Shard is simply the name of the schema where the data is stored. We'll learn more about shards later.</dd>

</dl>

#### Your First Repository

You've created your first class and registered it in AggregateTypes, but there's still one more thing needed before you can persist Person objects to the database. In NoSQLServer, for each class (Aggregate) you create, you'll also need to create a "repository" to save and retrieve objects of your new type. Fortunately you don't actually have to implement methods to insert, update or delete instances. NoSQLServer takes care of that for you. You tap into that functionality by extending the RepositoryBase class when you create your specific repositories. Let's create the PersonRepository to work with the Person object you've already created.

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DomainObjects.Enrollment;
    using NoSQLServer;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using NoSQLServer.Repository;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;
    namespace DomainRepositories
    {
        namespace Enrollment 
        {
            public class PersonRepository : RepositoryBase
            {
                public PersonRepository() : base (new Person())
                {
                }
                public Person GetPersonByID (long ID)
                {
                    Person p = (Person)formatter.Deserialize(readRepo.GetFetchAggregateByID().Execute(ID, Shard));
                    p.AggregateID = ID;
                    return p;
                }
            }
        }
    }

There are only a few things you'll need to do to make your repository work correctly:  

1.  You could name your repository anything but by convention it should be the same name as your type plus the word Repository. For example PersonRepository.
2.  Inherit from RepositoryBase, which means you'll need to make a reference to NoSQLServer.Repository.
3.  Create zero-argument constructor and then invoke the base class's constructor passing to it a new, empty instance of your type (Person, in this case)
4.  Implement a Get(your class name here)ByID method, such as GetPersonByID. Notice that AggregateIDs and AggregateTypeIDs are always "long" in C# and BIGINT in SQL. Fortunately, you'll never have to worry abotu dealing with anything in SQL :) You can literally copy and paste the code above in GetPersonID into _any_ class you create (in its GetByID method) and simply change "Person" to your type and it will work.

#### Putting It All Together

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DomainObjects.Enrollment;
    using NoSQLServer;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using NoSQLServer.Repository;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;
    public class PersonRepository : RepositoryBase
    {
        public static void Main(string[] args)
        {
            var repo = new PersonRepository();
            var person = new Person();
            person.FirstName = "John";
            person.LastName = "Doe";
            person.SSN = "123456789";
            person.BirthDate = new DateTime(1984,1,12);
            person.Gender = "M";
            long ID = repo.Save(person);
            var anotherPerson = repo.GetPersonByID(ID);
            Console.WriteLine(anotherPerson.FirstName);   // prints "John" 
        }
    }

#### Adding a Field to an Existing Class

This task could not be easier. Simply add the new property to the class and re-deploy the software. That's it. The new property will return null for every saved object of the modified type, so if you want a default value other than null, simply write some code in the property to return something else if it is null. For example:

<pre>    `using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NoSQLServer;
    namespace DomainObjects
    {
        namespace Enrollment
        {

           [Serializable] 
            public class Person  :  AggregateBase  
            {

                // other fields omitted for brevity...
                private string _NewField ;
                public string NewField
                {
                    get 
                    {
                        if (_NewField == null)
                            return String.Empty;
                        else
                            return  _NewField;
                    }
                    set
                    {
                       _NewField = value;
                    }
                }
                public override string GetUniqueKey()
                {
                    return SSN;
                }
                public override Dictionary<string>GetIndexes()
                {
                    return   new Dictionary<string, string>();
                }

            }
        }
    }` 
    </pre>

#### Changing the Name of an Existing Field

How many times have you seen a column mispelled in a table and thought "well, it *can* be fixed now, but it will be a pain.."? The second part of that pain is changing whatever code has also been written using the incorrect name. The first part is changing the column name. SQLServer 2016 finally introduces the ability to rename a column, but until then you must jump through hoops of selecting data into a new table with the correct column name then dropping and recreating the original table with the correct column name and then selecting back into it.

This is much simpler in NoSQLServer.

**BEFORE**

<pre>    `using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NoSQLServer;
    namespace DomainObjects
    {
        namespace Enrollment
        {

            [Serializable] 
            public class Person  :  AggregateBase  
            {

                // other fields omitted for brevity...
                private string _FirstNaem ;
                public string FirstNaem
                {
                    get 
                    {
                        return  _FirstNaem;
                    }
                    set
                    {
                        _FirstNaem = value;
                    }
                }
                public override string GetUniqueKey()
                {
                    return SSN;
                }
                public override Dictionary<string>GetIndexes()
                {
                    return   new Dictionary<string, string>();
                }

            }
        }
    }` 
    </pre>

**AFTER**

<pre>    `using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NoSQLServer;
    namespace DomainObjects
    {
        namespace Enrollment
        {

            [Serializable] 
            public class Person   :  AggregateBase  
            {

                // other fields omitted for brevity...

                // NOTE: This fieldname has been corrected. DO NOT change the private variable name...
                private string _FirstNaem ;     // step 1:   leave the PRIVATE variable alone....
                public string FirstName         // step 2:   change the PUBLIC property name to the correct spelling... THAT'S IT!
                {
                    get 
                    {
                        return  _FirstNaem;
                    }
                    set
                    {
                        _FirstNaem = value;
                    }
                }
                public override string GetUniqueKey()
                {
                    return SSN;
                }
                public override Dictionary<string>GetIndexes()
                {
                    return   new Dictionary<string, string>();
                }

            }
        }
    }` 
    </pre>

#### Deleting a field

The quick and dirty way of deleting a field is simply to delete the property from the class and redeploy the software... voila! The field is gone. However the data is still there. In order to delete the field AND get rid of the data it stored, you'll need to set it to null or an empty string, whatever makes sense for the field type, then save the object. You'll do this using a one-time program, then redeploy the class without the property. Now the data and the property are gone.

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DomainObjects.Enrollment;
    using NoSQLServer;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using NoSQLServer.Repository;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;
    public class PersonRepository : RepositoryBase
    {
        public static void Main(string[] args)
        {
            var repo = new PersonRepository();
            AggregateTypeInfo info = AggregateType.GetInfo(new Person());
            // simply loop through every person object and set the unwaanted field to 
            // string.Empty (or null or whatever doesn't take up much space...)
            foreach(Person p in repo.GetAll<Person>(info.ID,info.Shard)
            {
                // FavoriteColor was, in hindsight, probably not a necessary bit of information... let's get rid of it.
                p.FavoriteColor = string.Empty;       
                repo.Save(p);
            }
        }
    }

#### What Can't I (Easily) Change?

As you've seen it is _easy_ to add, remove and rename a field in NoSQLServer. There are a couple of types of changes that require a bit more effort so it's important to make sure you don't accidentally find yourself in the position of needing to to make these changes. If you do, however, it's not the end of the world... it just takes a little more effort.

The first case is where you have the field you want, named the way you want, but the "type" is not what it should have been. For instance, perhaps you made something a double and it should have been an int, or a string instead of a DateTime.

                // First, change this...
                [Serializable] 
                public class Person   :  AggregateBase  
                {

                    // other fields omitted for brevity...

                    // NOTE: This fieldname has been corrected. DO NOT change the private variable name...

                    private double _NumberOfPets ;      
                    public double NumberOfPets         
                    {
                        get 
                        {
                            return  _NumberOfPets;
                        }
                        set
                        {
                                _NumberOfPets = value;
                        }
                    }
                    public override string GetUniqueKey()
                    {
                        return SSN;
                    }
                    public override Dictionary<string>GetIndexes()
                    {
                        return   new Dictionary<string, string>();
                    }

                }
            }
            // To this...
            [Serializable] 
            public class Person   :  AggregateBase  
            {

                // other fields omitted for brevity...

                // NOTE: This fieldname has been corrected. DO NOT change the private variable name...

                private double _NumberOfPets ;      
                public double NumberOfPets         
                {
                    get 
                    {
                        return  _NumberOfPets;
                    }
                    set
                    {
                            _NumberOfPets = value;
                    }
                }
                private double _NewNumberOfPets ;      
                public double NewNumberOfPets         
                {
                    get 
                    {
                        return  _NewNumberOfPets;
                    }
                    set
                    {
                            _NewNumberOfPets = value;
                    }
                }
                public override string GetUniqueKey()
                {
                    return SSN;
                }
                public override Dictionary<string>GetIndexes()
                {
                    return   new Dictionary<string, string>();
                }

            }
        }
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DomainObjects.Enrollment;
    using NoSQLServer;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using NoSQLServer.Repository;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;
    public class PersonRepository : RepositoryBase
    {
        public static void Main(string[] args)
        {
            var repo = new PersonRepository();
            AggregateTypeInfo info = AggregateType.GetInfo(new Person());

            foreach(Person p in repo.GetAll<Person>(info.ID,info.Shard)
            {

                p.NewNumberOfPets = (int)p.NumberOfPets;
                p.NumberOfPets = null;
                repo.Save(p);
            }
        }
    }
        // Finally, "rename" the field.

        [Serializable] 
        public class Person   :  AggregateBase  
        {

            // other fields omitted for brevity...

            // NOTE: This fieldname has been corrected. DO NOT change the private variable name...

            private int _NewNumberOfPets ;      // private name remains the temp name...
            public int NumberOfPets             // public name is the same name as before...
            {
                get 
                {
                    return  _NewNumberOfPets;
                }
                set
                {
                        _NewNumberOfPets = value;
                }
            }
            public override string GetUniqueKey()
            {
                return SSN;
            }
            public override Dictionary<string>GetIndexes()
            {
                return   new Dictionary<string, string>();
            }

        }
    }

The other thing that is not easily undone is create a new object deploy it and then realize that it does not have the namespace you would like. Namespace is a _critically_ important part of NoSQLServer because it is how objects are uniquely defined and differentiated from each other. The best defense is to have a good namespace convention in place to follow it and to type carefully. See Joe if you need to correct a namespace problem after it has deployed to production. In test or dev we can simply delete all the new occurrences and fix the namespace.

#### Singleton Objects

Much of the time you'll store business domain objects, things like People, Employers, Families, Invoices, Eligibility, Payments, etc., but NoSQLServer also makes it fast and convenient to create what would traditionally be a "lookup table". You can do this simply creating an aggregate having a property type of Dictionary%lt;string,string> in which you store the keys (code) and value for each code.

Here's an example of member/dependent relationship codes:

<pre>    `namespace DomainObjects.Enrollment
{
    [Serializable]
    public class RelationshipCodes  : AggregateBase
    {
        private Dictionary<string, string> _Codes = new Dictionary<string, string>();
        public dictionary<string, string> Codes
        {
	    get 
	    {
            return _Codes;
	    }
	    set
	    {
            _Codes = value;
	    }
        }

        public string GetDescription(string code)
        {
            string result = string.Empty;
            result = Codes[code];
            if (result == null)
            {
                result = "Unknown code";
            }
            return result;
        }
        public override string GetUniqueKey()
        {
            return "Singleton";
        }
         public override dictionary<string,string> GetIndexes()
        {
            return new Dictionary<string, string>();
        }
    }
}

// do the initial load like this:

        // One time code run during deployment (like a 'one-time script')
        public static void Main(string[] args)
        {
            RelationshipCodesRepository repo = new RelationshipCodesRepository();
            RelationshipCodes lookup = new RelationshipCodes();
            lookup.Codes["M"] = "Member";
            lookup.Codes["W"] = "Wife";
            lookup.Codes["H"] = "Husband";
            lookup.Codes["D"] = "Daughter";
            lookup.Codes["S"] = "Son";
            lookup.Codes["SS"] = "Step-Son";
            lookup.Codes["SD"] = "Step-Daughter";
            repo.Save(lookup);
        }

// using the code in an ASP.NET MVC view

<td>
     @codes.GetDescription(fm.Relationship)
</td>
<td>
     @Html.DisplayFor(modelItem => fm.Person.LastName)
</td>` 
</pre>

#### The AggregateTypeInfo Object

The need to know about this object will hopefully be eliminated by abstracting it up into the framework. For now all you need to know is that if you're calling a method that wants to know the AggregateTypeId or Shard information for an AggregateType, you can get the information as shown below. In this example we're getting it for a Person object:

<pre>        AggregateTypeInfo info = AggregateType.GetInfo(new Person.GetType().FullName);
        info.ID // = 7
        info.Shard = "NoSQLServer_01"
    </pre>

#### Nested Objects

Once we ditch traditional RDBMS table structures, one of the most powerful things we can do, in addition to saving objects of several types all to the same table, is to _nest_ one or more objects within another object. Whenever you think of a "header/detail" scenario, think "nested objects". Examples are everywhere:

*   Plans inside of a Participation Agreement
*   People inside a Family
*   Employees within an Employer

Class design is very flexible: you don't _have to_ nest objects, but you can. You can also create separate objects and store references to them. If you go this route, it's a bit different than header/detail in RDBMS. In that approach, each detail row stores a foreign key to the header. In this the NoSQL approach, the detail document is a single document holding _all_ detail records with the primary (header) object holding a single pointer to the detail collection. This allows all details to be read with a single read operation and not clutter the definition of the object itself with the collection if it makes more sense for the collection to be related to, but not part of the object definition.

#### Cascading Updates

The first thing a skeptic might think of when they first hear about nested objects is "what about stale data when the object is updated?" It's a legitimate question and one that NoSQLServer addresses out of the box In short, when you choose to cache objects of one type in an object of another type, you can ask the framework to take care of this for you by simply registering a class to be called when an object of a certain type is updated. The framework will asynchronously create an instance of an updater class that you create and supply it with the AggregateID of the object that changed. Your updater class is then invoked by the framework and you can update any objects that require updating. Consider that the Family class contains a List of Person objects and all the objects are serialized together into the database as a binary blob. Now, suppose that one of the Person objects is updated. How do you insure that the same person objects, cached inside of a Family object, are updated? You only need to do three things to have the framework take care of this for you:

1.  Write an updater class
2.  Register the updater class in the CascadingUpdates table
3.  Add two methods to the class that will be udpated(Family): bool RequiresRefresh(Person p) and bool Refresh(Person p).

Here is the updater class that is called to update Families when a Person is updated:

<pre>    
    `

    public class UpdateEmployersWithPerson : ICascadingUpdate
    {
        public void Update(long AggregateID)
        {
            PersonRepository personRepo = new PersonRepository();
            Person p = personRepo.GetPersonByID(AggregateID);
            EmployerRepository empRepo = new EmployerRepository();
            AggregateTypeInfo info = AggregateType.GetInfo("DomainObjects.Employers.Employer");
            foreach (Employer employer in empRepo.GetAll<employer>(info.ID, info.Shard.Trim()))
            {
                if (employer.RefreshRequired(p))
                {
                    employer.Refresh(p);
                    empRepo.Save(employer);
                }
            }
        }
    }</employer> `
    </pre>

Next, register the updater with the AggregateID of the AggregateTypeID whose change initiates the cascasing update. In this example we use the AggregateTypeID of "Person"

<pre>
    insert into NoSQLServer_MASTER.CascadingUpdates 
    (AggregateTypeID, CascadingUpdater) 
    values 
    (5,'DomainUpdaters.Enrollment.UpdateFamiliesWithPerson')

</pre>

Lastly, implement these simple methods that are called from your updater class. The implentation may vary for your particular case.

            // used to refresh cached Person objects with fresh person objects
            public void Refresh(Person p)
            {
                FamilyMember member = FamilyMembers.Where(x => x.Person.AggregateID  == p.AggregateID).First();
                member.Person = p;
            }
            public bool RefreshRequired(Person p)
            {
                return FamilyMembers.Where(x => x.Person.AggregateID == p.AggregateID).Count() > 0;
            }

#### Understanding Unique Keys

#### Understanding Indexes

#### Creating Indexes

#### Understanding Sharding

#### Creating a New Shard

#### Commands and Invokers

#### Single Writer Pattern

#### Unit of Work Pattern

#### Aggregate Events

#### Replaying Events in the Debugger

#### Maintaining a BI Database From NoSQLServer

#### Assemblies and References

#### Browsing Object Data (like in Query Analyzer)

#### The Classes in This Demo App

There are several classes in this demonstration web app, but four main classes that have their own aggregate types:

<dl>

<dt>Person</dt>

<dd>Contains the basic information about a Person</dd>

<dt>Family</dt>

<dd>Contains information at the Family (Subscriber) level, such as Subscriber Number _and_ contains a collection of FamilyMember objects each containing a Person object for this family. Reading the Family object from the DB retrieves all members in the same read operation.</dd>

<dt>Employer</dt>

<dd>Contains information at the Employer , such as Employer Name and Employer Number _and_ contains a collection of Employee objects for this Employer. Reading the Employer object from the DB retrieves all employees in the same read operation.</dd>

<dt>RelationshipCodes</dt>

<dd>This is an example of using a Singleton class as a lookup field translator.</dd>

</dl>

<pre>
       `using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQLServer; //.Adapters
using Commands;
using System.Runtime.Serialization.Formatters.Binary;
namespace DomainObjects
{
    namespace Enrollment
    {
        [Serializable]
        public class Person : AggregateBase 
        {

            private string _FirstName ;
            public string FirstName
            {
                get 
                {
                    return  _FirstName;
                }
                set
                {
                    _FirstName = value;
                }
            }

            private string _LastName ;
            public string LastName
            {
                get 
                {
                    return  _LastName;
                }
                set
                {
                    _LastName = value;
                }
            }

            private string _Gender ;
            public string Gender
            {
                get 
                {
                    return  _Gender;
                }
                set
                {
                    _Gender = value;
                }
            }

            private DateTime _BirthDate ;
            public DateTime BirthDate
            {
                get 
                {
                    return  _BirthDate;
                }
                set
                {
                    _BirthDate = value;
                }
            }
            private string _GovernmentIssuedID ;
            public string GovernmentIssuedID
            {
                get 
                {
                    return  _GovernmentIssuedID;
                }
                set
                {
                    _GovernmentIssuedID = value;
                }
            }

            private string _GovernmentIssuedIDType ;
            public string GovernmentIssuedIDType
            {
                get 
                {
                    return  _GovernmentIssuedIDType;
                }
                set
                {
                    _GovernmentIssuedIDType = value;
                }
            }
            private string _MiddleName;
            public string MiddleName
            {
                get
                {
                    return _MiddleName ?? string.Empty;
                }
                set
                {
                    _MiddleName = value;
                }
            }
            public Person() : base()
            {
            }

            public override string GetUniqueKey()
            {
                if ( GovernmentIssuedID != null && GovernmentIssuedID.Trim() != "" && GovernmentIssuedID.Trim() != "000000000")
                    return GovernmentIssuedID;
                else 
                    return Guid.NewGuid().ToString();
            }
            public override Dictionary<string, string> GetIndexes()
            {
                Dictionary<string, string> indexes = new Dictionary<string, string>();
                IndexData indexData = new IndexData();
                return indexes;
            }
            public static string ToStringHeader()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('\r').Append('\n');
                sb.Append(string.Format(" {0,20}   {1,20}   {2,20}     {3,10}    {4,9}", 
                                "First Name", "Middle Name", "Last Name", "Birth Date", "SSN/ITIN"));
                sb.Append('\r').Append('\n');
                sb.Append(string.Format(" {0,20}   {1,20}   {2,20}     {3,10}    {4,9}",
                         "==========", "===========", "=========", "==========", "========"));
                return sb.ToString();
            }
            public override string ToString()
            {
                return string.Format(" {0,20}   {1,20}   {2,20}     {3,10}    {4,9}", 
                    FirstName, MiddleName, LastName, BirthDate.ToShortDateString(), GovernmentIssuedID);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObjects;
using NoSQLServer; //.Adapters
using Commands;
using System.Runtime.Serialization.Formatters.Binary;
namespace DomainObjects 
{
    namespace Enrollment
    {
        [Serializable]
        public class FamilyMember
        {
            public Person Person ;
            public string Relationship  ;
            public DateTime From ;
            public DateTime To ;
        }
        [Serializable]
        public class Family : AggregateBase 
        {
            private string _SubscriberNumber = string.Empty;
            public string SubscriberNumber
            {
                get
                {
                    return _SubscriberNumber;
                }
                set
                {
                    _SubscriberNumber = value;
                }
            }

            public string GetSubscriberNumber()
            {
                if (SubscriberNumber != null && SubscriberNumber != string.Empty)
                    return SubscriberNumber;
                else
                    return GetGovernmentID();
            }

<div style="background-color:lightyellow">
            public List<familymember> FamilyMembers = new List<familymember>();
            public List<familymember> EligibleDependents
            {
                get
                {
                    return EligibleDependentsAsOf(DateTime.Now.Date);
                }
            }
            </div>

            public List<familymember> EligibleDependentsAsOf(DateTime theDate)
            {
                return FamilyMembers.Where(x => x.From.CompareTo(theDate) <= 0 && x.To.CompareTo(theDate) >= 0).ToList<familymember>();
            }
            public Person Participant
            {
                get
                {
                    return FamilyMembers.Where(x => x.Relationship == "M").First().Person;
                }
            }
            public string GetGovernmentID()
            {
                return Participant.GovernmentIssuedID; 
            }
            public override string ToString()
            {
               StringBuilder sb = new StringBuilder();
               sb.Append(string.Format("Subscriber Number:  {0}", SubscriberNumber)).Append('\r').Append('\n'); 
               sb.Append(string.Format("Aggregate ID     :  {0}", AggregateID)).Append('\r').Append('\n').Append('\r').Append('\n');

               foreach(FamilyMember mbr in FamilyMembers)
               {
                   sb.Append(string.Format("{0,20} {1,20} {2,20} {3,10}  {4,10}", mbr.Person.FirstName, mbr.Person.MiddleName, mbr.Person.LastName, mbr.Relationship, mbr.Person.GovernmentIssuedID)).Append('\r').Append('\n');
               }
               return sb.ToString();
            }

            // used to refresh cached Person objects with fresh person objects
            public void Refresh(Person p)
            {
                FamilyMember member = FamilyMembers.Where(x => x.Person.AggregateID  == p.AggregateID).First();
                member.Person = p;
            }
            public bool RefreshRequired(Person p)
            {
                return FamilyMembers.Where(x => x.Person.AggregateID == p.AggregateID).Count() > 0;
            }
            // required by NoSQLServer framework in every Aggregate object.
            public override string GetUniqueKey()
            {
                return GetSubscriberNumber();
            }
            public override Dictionary<string, string > getindexes()
            {
                 dictionary<string, string> indexes=new dictionary<string, string>();
                 indexdata indexdata=new indexdata();
                 indexdata.add("memberssn", participant.governmentissuedid);
                 indexdata.compute();
                 indexes.add(indexdata.columnnames, indexdata.datavalues);
                 return indexes;
             }
         }
    }
 }
         using System;
         using System.collections.generic;
         using System.linq;
         using System.text;
         using System.threading.tasks;
         using Domainobjects.Enrollment;
         using NoSQLServer;
         namespace Domainobjects
         {
         namespace Employers
         {
         [serializable]
         public class Employer
         aggregatebase
         {

<div style="background-color:lightyellow">
           private List<employee> _Employees = new List<employee>();

            public List<employee> Employees
            {
                get
                {
                    return  _Employees;
                }
                set
                {
                    _Employees = value;
                }
            }
            </div>

            public void AddEmployee(Employee employee)
            {
                Employees.Add(employee);
            }
            private string _EmployerName ;
            public string EmployerName
            {
                get
                {
                    return  _EmployerName;
                }
                set
                {
                    _EmployerName = value;
                }
            }

            private int _EmployerID ;
            public int EmployerID
            {
                get
                {
                    return  _EmployerID;
                }
                set
                {
                    _EmployerID = value;
                }
            }
            public override string GetUniqueKey()
            {
                return EmployerID.ToString();
            }

            public override Dictionary<string, string> GetIndexes()
            {
                return new Dictionary<string, string>();
            }
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("Empoloyer Name: {0}", _EmployerName)).Append('\r').Append('\n');
                foreach (Employee employee in Employees)
                {
                    sb.Append(string.Format("Employee Name: {0}", employee.Person.GovernmentIssuedID + " " +  employee.Person.FirstName.Trim() + " " + employee.Person.LastName));
                }
                return sb.ToString();
            }
            public void Refresh(Person p)
            {
                Employee employee = Employees.Where(x => x.Person.AggregateID == p.AggregateID).First();
                employee.Person = p;
            }
            public bool RefreshRequired(Person p)
            {
                return Employees.Where(x => x.Person.AggregateID == p.AggregateID).Count() > 0;
            }
        }
        [Serializable]
        public class Employee
        {
            public Person Person;
            public DateTime HireDate;
            public List<ReportingStatus> ReportingHistory=new list<ReportingStatus>();
        }
        [Serializable]
        public class ReportingStatus
        {
            public Person Employee;
            //public EmployerGroup Group;
            public string ReportingCode;
            public DateTime ReportedDate;
            public DateTime ActionDate;
            public DateTime EffectiveDate;
        }
    }
}` 
</pre>

#### Help Wanted

</div>
