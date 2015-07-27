using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb;
using Pls.SimpleMongoDb.DataTypes;
using Pls.SimpleMongoDb.Querying;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Session
{
    [TestClass]
    public class SessionFindTests
        : TestBase
    {
        private const string DbName = Constants.TestDbName;
        private const string PersonsCollectionName = Constants.Collections.PersonsCollectionName;

        [TestMethod]
        public void Find_UsingMatchingValueInArray_ReturnsMatchingPost()
        {
            var collectionName = "BlogEntries";
            var documents = new[]
            {
                new { Title = "MongoDb and testing", Tags = new[] { "MongoDb", "Testing" } }, 
                new { Title = "MongoDb tutorial", Tags = new[] { "MongoDb", "Tutorial" } }
            };
            TestHelper.InsertDocuments(collectionName, documents);

            using(var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var tutorial = session[DbName][collectionName].FindOneInfered(new {Title = ""}, @"{Tags : 'Tutorial'}");

                Assert.AreEqual("MongoDb tutorial", tutorial.Title);
            }
        }

        [TestMethod]
        public void Find_UsingAllOperator_ReturnsTwoMatchingPosts()
        {
            var collectionName = "BlogEntries";
            var documents = new[]
            {
                new { Title = "MongoDb other", Tags = new[] { "MongoDb", "Misc" } }, 
                new { Title = "MongoDb and testing tutorial", Tags = new[] { "MongoDb", "Testing", "Tutorial" } }, 
                new { Title = "MongoDb tutorial", Tags = new[] { "MongoDb", "Tutorial" } }
            };
            TestHelper.InsertDocuments(collectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var tutorials = session[DbName][collectionName].FindInfered(new { Title = "" }, @"{Tags : {$all :['Tutorial']}}");

                Assert.AreEqual(2, tutorials.Count);
            }
        }

        [TestMethod]
        public void Find_UsingMatchingValueInArray_ReturnsTwoMatchingPosts()
        {
            var collectionName = "BlogEntries";
            var documents = new[]
            {
                new { Title = "MongoDb other", Tags = new[] { "MongoDb", "Misc" } }, 
                new { Title = "MongoDb and testing tutorial", Tags = new[] { "MongoDb", "Testing", "Tutorial" } }, 
                new { Title = "MongoDb tutorial", Tags = new[] { "MongoDb", "Tutorial" } }
            };
            TestHelper.InsertDocuments(collectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var tutorials = session[DbName][collectionName].FindInfered(new { Title = "" }, @"{Tags : 'Tutorial'}");

                Assert.AreEqual(2, tutorials.Count);
            }
        }

        [TestMethod]
        public void Find_UsingSimoInOperator_TwoOfThreeItemsReturned()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Adam", Age = 55},
                                new Person {Name = "Sue", Age = 55},
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(Query.New(q => q["Name"].In("Daniel", "Sue")));

                var danielAndSueFound = persons.Where(p => new[] {"Daniel", "Sue"}.Contains(p.Name)).Count() == 2;
                Assert.AreEqual(2, persons.Count);
                Assert.IsTrue(danielAndSueFound);
            }
        }

        [TestMethod]
        public void Find_UsingJsonInOperator_TwoOfThreeItemsReturned()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Adam", Age = 55},
                                new Person {Name = "Sue", Age = 55},
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(@"{Name : { $in : [""Daniel"", ""Sue""] } }");

                var danielAndSueFound = persons.Where(p => new[] { "Daniel", "Sue" }.Contains(p.Name)).Count() == 2;
                Assert.AreEqual(2, persons.Count);
                Assert.IsTrue(danielAndSueFound);
            }
        }

        [TestMethod]
        public void Find_UsingSimoWhereOperator_TwoOfThreeItemsReturned()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Adam", Age = 55},
                                new Person {Name = "Sue", Age = 55},
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(Query.New(q => q.Where("this.Name == 'Daniel' || this.Name == 'Sue'")));

                var danielAndSueFound = persons.Where(p => new[] { "Daniel", "Sue" }.Contains(p.Name)).Count() == 2;
                Assert.AreEqual(2, persons.Count);
                Assert.IsTrue(danielAndSueFound);
            }
        }

        [TestMethod]
        public void Find_UsingJsonWhereOperator_TwoOfThreeItemsReturned()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Adam", Age = 55},
                                new Person {Name = "Sue", Age = 55},
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(@"{$where : ""this.Name == 'Daniel' || this.Name == 'Sue'""}");

                var danielAndSueFound = persons.Where(p => new[] { "Daniel", "Sue" }.Contains(p.Name)).Count() == 2;
                Assert.AreEqual(2, persons.Count);
                Assert.IsTrue(danielAndSueFound);
            }
        }

        [TestMethod]
        public void Find_UsingDocumentSelector_SingleItemReturned()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29, WorkDays = new[] {1, 1, 1, 0, 1, 0, 0}},
                                new Person {Name = "Lazy John", Age = 55, WorkDays = new[] {0, 1, 0, 1, 0, 0, 0}},
                                new Person {Name = "Nobel Adam", Age = 65, WorkDays = new[] {1, 1, 1, 1, 1, 1, 1}},
                                new Person {Name = "Sue", Age = 20, WorkDays = new[] {1, 1, 1, 1, 1, 0, 0}}
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(new { WorkDays = new[] { 1, 1, 1, 1, 1, 1, 1 } });

                Assert.AreEqual("Nobel Adam", persons[0].Name);
            }
        }

        [TestMethod]
        public void Find_UsingWhereOperator_SingleItemReturned()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Lazy John", Age = 55},
                                new Person {Name = "Nobel Adam", Age = 65},
                                new Person {Name = "Sue", Age = 20}
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(Query.New(q => q.Where("this.Age > 20 && this.Age < 65")));

                Assert.AreEqual("Daniel", persons[0].Name);
                Assert.AreEqual("Lazy John", persons[1].Name);
            }
        }

        [TestMethod]
        public void Find_UsingRegExOperator_ThreeItemsReturned()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Daniel1", Age = 55},
                                new Person {Name = "Daniel2", Age = 65},
                                new Person {Name = "Sue", Age = 20}
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var session = new SimoSession(TestHelper.CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(new { Name = new SimoRegex("^Dan.*") });

                Assert.AreEqual(3, persons.Count, "Wrong number of persons returned.");
                Assert.AreEqual("Daniel", persons[0].Name);
                Assert.AreEqual("Daniel1", persons[1].Name);
                Assert.AreEqual("Daniel2", persons[2].Name);
            }
        }
    }
}