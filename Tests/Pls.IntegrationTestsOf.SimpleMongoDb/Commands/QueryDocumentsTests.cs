using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb.Commands;
using Pls.SimpleMongoDb.DataTypes;
using Pls.SimpleMongoDb.Querying;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Commands
{
    [TestClass]
    public class QueryDocumentsTests
        : TestBase
    {
        [TestMethod]
        public void QuerySinglePerson_AnonymousTypeQueryWithString_ItemReturned()
        {
            var person = new Person { Name = "Daniel", Age = 29 };
            TestHelper.InsertDocument(Constants.Collections.PersonsCollectionName, person);

            using (var cn = TestHelper.CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = new { Name = "Daniel" }
                                   };
                queryCommand.Execute();

                Assert.AreEqual(1, queryCommand.Response.NumberOfDocuments);
            }
        }

        [TestMethod]
        public void QuerySinglePerson_AnonymousTypeQueryWithDateTime_ItemReturned()
        {
            var tempDate = new DateTime(2010, 1, 1, 10, 02, 03, 04);
            var person = new Person { Name = "Daniel", Age = 29, TempDate = tempDate };
            TestHelper.InsertDocument(Constants.Collections.PersonsCollectionName, person);

            using (var cn = TestHelper.CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = new { TempDate = tempDate }
                                   };
                queryCommand.Execute();

                Assert.AreEqual(1, queryCommand.Response.NumberOfDocuments);
            }
        }

        [TestMethod]
        public void QueryManyPersons_NoQuery_ReturnsAll()
        {
            var tempDate = new DateTime(2010, 1, 1, 10, 02, 03, 04);
            var persons = new[]
                          {
                              new Person { Name = "Daniel", Age = 29, TempDate = tempDate },
                              new Person { Name = "Daniel", Age = 30, TempDate = tempDate }
                          };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, persons);

            using (var cn = TestHelper.CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName
                                   };
                queryCommand.Execute();

                Assert.AreEqual(2, queryCommand.Response.NumberOfDocuments);
            }
        }

        [TestMethod]
        public void QueryManyPersons_WhereClauseUsingJson_ReturnsAllExceptOne()
        {
            var person = new[]
                         {
                             new Person { Name = "Daniel1", Age = 29 },
                             new Person { Name = "Daniel2", Age = 29 },
                             new Person { Name = "Daniel3", Age = 30 }
                         };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, person);

            using (var cn = TestHelper.CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = @"{$where : ""this.Name.indexOf('Daniel') > -1 && this.Age < 30""}"
                                   };
                queryCommand.Execute();

                Assert.AreEqual(2, queryCommand.Response.NumberOfDocuments);
            }
        }

        [TestMethod]
        public void QueryManyPersons_WhereClauseUsingJsonObject_ReturnsAllExceptOne()
        {
            var person = new[]
                         {
                             new Person { Name = "Daniel1", Age = 29 },
                             new Person { Name = "Daniel2", Age = 29 },
                             new Person { Name = "Daniel3", Age = 30 }
                         };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, person);

            using (var cn = TestHelper.CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = new SimoJson(@"{$where : ""this.Name.indexOf('Daniel') > -1 && this.Age < 30""}")
                                   };
                queryCommand.Execute();

                Assert.AreEqual(2, queryCommand.Response.NumberOfDocuments);
            }
        }

        [TestMethod]
        public void QueryManyPersons_WhereClauseUsingWhereOperator_ReturnsThreePersons()
        {
            var person = new[]
                         {
                             new Person { Name = "Daniel1", Age = 28 },
                             new Person { Name = "Daniel2", Age = 29 },
                             new Person { Name = "Daniel3", Age = 30 },
                             new Person { Name = "Daniel4", Age = 31},
                             new Person { Name = "Daniel5", Age = 32}
                         };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, person);

            using (var cn = TestHelper.CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = Query.New(q => q.Where("this.Name.indexOf('Daniel') > -1 && this.Age <= 30"))
                                   };
                queryCommand.Execute();

                Assert.AreEqual(3, queryCommand.Response.NumberOfDocuments);
            }
        }

        [TestMethod]
        public void QueryManyPersonsUsingCursor_SpecificNumberOfDocumentsToReturnGivesNoCursor_SpecifiedNumberOfPersonsAreReturned()
        {
            var person = new[]
                         {
                             new Person { Name = "Daniel1", Age = 28 },
                             new Person { Name = "Daniel2", Age = 29 },
                             new Person { Name = "Daniel3", Age = 30 },
                             new Person { Name = "Daniel4", Age = 31},
                             new Person { Name = "Daniel5", Age = 32}
                         };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, person);

            using (var cn = TestHelper.CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                {
                    FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                    NumberOfDocumentsToReturn = 2
                };
                queryCommand.Execute();

                Assert.AreEqual(2, queryCommand.Response.NumberOfDocuments);
            }
        }
    }
}