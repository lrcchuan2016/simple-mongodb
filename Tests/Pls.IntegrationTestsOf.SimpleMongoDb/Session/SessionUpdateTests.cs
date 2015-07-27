using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Session
{
    [TestClass]
    public class SessionUpdateTests
        : TestBase
    {
        private const string DbName = Constants.TestDbName;
        private const string PersonsCollectionName = Constants.Collections.PersonsCollectionName;

        [TestMethod]
        public void Update_EmptyDb_UpsertsDocument()
        {
            var document = new { Name = "Daniel" };

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                session[DbName][PersonsCollectionName].Update(document, document);
            }

            var numOfStoredDocuments = TestHelper.GetDocumentCount(Constants.Collections.PersonsCollectionName);
            Assert.AreEqual(1, numOfStoredDocuments);
        }

        [TestMethod]
        public void Update_UsingAnonymousDocument_TwoOfFourItemsUpdated()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel1", Age = 29},
                                new Person {Name = "Daniel2", Age = 30},
                                new Person {Name = "Daniel3", Age = 31},
                                new Person {Name = "Daniel4", Age = 32}
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                session[DbName][PersonsCollectionName].UpdateMany(@"{$where : ""this.Age < 31""}", @"{$inc : {Age : 1}}");
            }

            var refetched = TestHelper.GetDocuments<Person>(Constants.Collections.PersonsCollectionName);
            Assert.AreEqual(30, refetched[0].Age);
            Assert.AreEqual(31, refetched[1].Age);
            Assert.AreEqual(31, refetched[2].Age);
            Assert.AreEqual(32, refetched[3].Age);
        }
    }
}