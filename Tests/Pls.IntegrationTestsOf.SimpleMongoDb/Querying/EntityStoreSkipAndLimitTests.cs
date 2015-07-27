using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Querying
{
    [TestClass]
    public class EntityStoreSkipAndLimitTests
        : TestBase
    {
        private const string DbName = Constants.TestDbName;

        private void SetupData(int numOfPersons)
        {
            var persons = new List<Person>();
            for (var i = 0; i < numOfPersons; i++)
                persons.Add(new Person { Name = "Daniel", Age = i + 1 });

            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, persons);            
        }

        [TestMethod]
        public void FindAll_SkipsTwoOf100_ReturnsRemaining98()
        {
            SetupData(100);

            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.FindAll<Person>(opts => opts.Skip(2));

                Assert.AreEqual(98, persons.Count);
                Assert.AreEqual(3, persons[0].Age);
            }
        }

        [TestMethod]
        public void FindAll_SkipsThreeOfTwo_ReturnsNoItems()
        {
            SetupData(3);

            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.FindAll<Person>(opts => opts.Skip(3));

                Assert.AreEqual(0, persons.Count);
            }
        }

        [TestMethod]
        public void FindAll_LimitsTo20Of100_Returns20()
        {
            SetupData(100);

            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.FindAll<Person>(opts => opts.Limit(20));

                Assert.AreEqual(20, persons.Count);
                Assert.AreEqual(1, persons[0].Age);
            }
        }

        [TestMethod]
        public void FindAll_LimitsTo20Of15_Returns15()
        {
            SetupData(15);

            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.FindAll<Person>(opts => opts.Limit(20));

                Assert.AreEqual(15, persons.Count);
            }
        }

        [TestMethod]
        public void FindAll_Skip10LimitTo10WhenTotalOf100_Returns10()
        {
            SetupData(100);

            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.FindAll<Person>(opts => opts.Skip(10).Limit(10));

                Assert.AreEqual(10, persons.Count);
                Assert.AreEqual(11, persons[0].Age);
            }
        }
    }
}