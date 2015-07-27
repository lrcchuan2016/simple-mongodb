using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb;
using Pls.SimpleMongoDb.DataTypes;
using Pls.SimpleMongoDb.Querying;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    /// <summary>
    /// This class is not intended to execute tests (event though it does this),
    /// but to ensure that some ApiExamples that are published online, are valid.
    /// </summary>
    [TestClass]
    public class ApiExamples
        : TestBase
    {
        private const string DbName = Constants.TestDbName;

        [TestMethod]
        public void Insert_Example()
        {
            //The hierarchy in MongoDB is:
            //- Db
            //  - Collection
            //    - Document
            //By using EntityStore this is somewhat abstracted away.

            //You do need a connection to create a session
            //The creating of Connections and Session is something you most likely
            //will put in a IoC-container or factory or something.
            var cn = TestHelper.CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                //If you are dealing with typed documents (e.g. Person)
                //you can use the generic API.
                //Then the name of the collection is the name of the
                //type, but pluralized.
                //If you don't want pluralization, call
                //session.SimoPluralizer.Disable(), or
                //replace the implementation of the 
                //SimoIoC.Instance.PluralizerResolver
                var person = new Person { Name = "Daniel", Age = 29 };
                entityStore.Insert(person);

                //If you are using non-typed documents you have to pass
                //the entityname string to the method you are using.
                var anonymousPerson = new { Name = "Daniel" };
                entityStore.Insert("Person", anonymousPerson);

                //So the EntityStore only allows for an abstraction
                //over the Db, Collection and Document hierarchy
                //and you can of course access these to.
                var db = session[DbName];
                var persons = db["Person"];
                persons.Insert(anonymousPerson);

                //The EntityStore also holds the Database that it wraps.
                //Of course you can obtain a collection using generics against the
                //database.
                var persons2 = entityStore.Database.GetCollection<Person>();
                persons2.Insert(person);

                var numOfStoredDocuments = entityStore.Count<Person>();
                Assert.AreEqual(4, numOfStoredDocuments);
            }
        }

        [TestMethod]
        public void Find_UsingRegex()
        {
            var cn = TestHelper.CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                entityStore.Insert(new Person { Name = "Daniel", Age = 29 });
                entityStore.Insert(new Person { Name = "Daniel1", Age = 45 });
                entityStore.Insert(new Person { Name = "Daniel1", Age = 55 });
                entityStore.Insert(new Person { Name = "Daniel2", Age = 65 });
                entityStore.Insert(new Person { Name = "Sue", Age = 20 });

                var refetched = entityStore.FindOne<Person>(new { Age = 45, Name = new SimoRegex("^Dan.*1") });

                Assert.AreEqual(45, refetched.Age);
                Assert.AreEqual("Daniel1", refetched.Name);
            }
        }

        [TestMethod]
        public void ParentChildReference_Example()
        {
            var cn = TestHelper.CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                //The parent generates a new _id when created.
                //That _id is then used in the reference which is attached to the child.
                //After that, you just store the items.
                var parent = new Parent { Name = "Daniel" };
                var fatherReference = entityStore.Reference<Parent>(parent._id);
                var child = new Child { Name = "Isabell", FatherReference = fatherReference };

                //You could of course have created the reference manually, but then you loose the
                //use of the pluralizer, and have to role-this on your own.
                //new SimoReference { CollectionName = "Parents", Id = parent._id };

                entityStore.Insert(parent);
                entityStore.Insert(child);

                var refetchedChild = entityStore.FindOne<Child>(new { child._id });

                Assert.AreEqual(fatherReference.Id, refetchedChild.FatherReference.Id);
                Assert.AreEqual(fatherReference.CollectionName, refetchedChild.FatherReference.CollectionName);
            }
        }

        [TestMethod]
        public void ParentChildReference_CustomId_Example()
        {
            var cn = TestHelper.CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                //This time we use anonymous type and custom Id's (Guids).
                var parent = new { _id = Guid.NewGuid(), Name = "Daniel" };
                var fatherReference = entityStore.Reference("Parent", parent._id);
                var child = new { _id = Guid.NewGuid(), Name = "Isabell", FatherReference = fatherReference };

                //You could of course have created the reference manually, but then you loose the
                //use of the pluralizer, and have to role-this on your own.
                //new SimoReference<Guid> { CollectionName = "Parents", Id = parent._id };

                entityStore.Insert("Parent", parent);
                entityStore.Insert("Child", child);

                var refetchedChild = entityStore.FindOneInfered(child, entityName: "Child", selector: new { child._id });

                Assert.AreEqual(fatherReference.Id, refetchedChild.FatherReference.Id);
                Assert.AreEqual(fatherReference.CollectionName, refetchedChild.FatherReference.CollectionName);
            }
        }

        [TestMethod]
        public void DisablePluralizer_Example()
        {
            var cn = TestHelper.CreateConnection();
            using (var session = new SimoSession(cn))
            {
                //Just call Disable on the Pluralizer
                //When you want to enable it again, call
                //Enable().
                var entityStore = new SimoEntityStore(session, DbName);
                entityStore.Session.Pluralizer.Disable();

                var person = new Person { Name = "Daniel" };
                entityStore.Insert(person);

                var refetched = entityStore.Database["Person"].FindOne<Person>(new { person.Name });

                Assert.IsNotNull(refetched);
            }
        }

        [TestMethod]
        public void SessionFactory_Example()
        {
            //So there's a verry simple SessionFactory inplace,
            //but you can of course make one of your own or
            //let an IoC return sessions.

            var sessionFactory = new SimoSessionFactory();
            using (var session = sessionFactory.GetSession(Constants.ConnectionStringName))
            {
                session[DbName].DropDatabase();
            }
        }
    }
}