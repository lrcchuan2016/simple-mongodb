using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb.Commands;
using Pls.SimpleMongoDb.Querying;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Commands
{
    [TestClass]
    public class DeleteDocumentsTests
        : TestBase
    {
        [TestMethod]
        public void DeleteAll_WithSelector_ZeroCountLeft()
        {
            var documents = new[]
                            {
                                new { Name = "Daniel", Age = 29 }, 
                                new { Name = "Daniel", Age = 30 }
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var cn = TestHelper.CreateConnection())
            {
                var deleteCommand = new DeleteDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Selector = new { Name = "Daniel" }
                                    };
                deleteCommand.Execute();
            }

            Assert.AreEqual(0, TestHelper.GetDocumentCount(Constants.Collections.PersonsCollectionName));
        }

        [TestMethod]
        public void DeleteAll_NullSelector_ZeroCountLeft()
        {
            var documents = new[]
                            {
                                new { Name = "Daniel", Age = 29 },
                                new { Name = "Daniel", Age = 30 }
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            using (var cn = TestHelper.CreateConnection())
            {
                var deleteCommand = new DeleteDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName
                                    };
                deleteCommand.Execute();
            }

            Assert.AreEqual(0, TestHelper.GetDocumentCount(new { Name = "Daniel" }, Constants.Collections.PersonsCollectionName));
        }

        [TestMethod]
        public void Delete_WhereClauseViaOperator_OneItemLeft()
        {
            var persons = new[]
                          {
                              new Person { Name = "Daniel1", Age = 29 },
                              new Person { Name = "Daniel2", Age = 29 },
                              new Person { Name = "Daniel3", Age = 30 }
                          };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, persons);

            using (var cn = TestHelper.CreateConnection())
            {
                var deleteCommand = new DeleteDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Selector = Query.New(q => q.Where("this.Name.indexOf('Daniel') > -1 && this.Age < 30"))
                                    };
                deleteCommand.Execute();
            }

            Assert.AreEqual(1, TestHelper.GetDocumentCount(Constants.Collections.PersonsCollectionName));
        }

        [TestMethod]
        public void Delete_WhereClauseViaJson_OneItemLeft()
        {
            var persons = new[]
                          {
                              new Person { Name = "Daniel1", Age = 29 },
                              new Person { Name = "Daniel2", Age = 29 },
                              new Person { Name = "Daniel3", Age = 30 }
                          };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, persons);

            using (var cn = TestHelper.CreateConnection())
            {
                var deleteCommand = new DeleteDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Selector = @"{$where : ""this.Name.indexOf('Daniel') > -1 && this.Age < 30""}"
                                    };
                deleteCommand.Execute();
            }

            Assert.AreEqual(1, TestHelper.GetDocumentCount(Constants.Collections.PersonsCollectionName));
        }
    }
}