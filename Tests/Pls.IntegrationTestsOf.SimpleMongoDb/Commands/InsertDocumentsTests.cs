using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb.Commands;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Commands
{
    [TestClass]
    public class InsertDocumentsTests
        : TestBase
    {
        [TestMethod]
        public void InsertSingle_AnonymousDocument_IsStoredAndAssignedId()
        {
            var document2Insert = new { Name = "Daniel", Age = 29 };

            using (var cn = TestHelper.CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Documents = new[] { document2Insert }
                                    };
                insertCommand.Execute();
            }

            var inferedTemplate = new { _id = SimoId.Empty };
            var refetched = TestHelper.GetDocument(document2Insert, inferedTemplate, Constants.Collections.PersonsCollectionName);
            Assert.AreNotEqual(SimoId.Empty, refetched._id);
        }

        [TestMethod]
        public void InsertSingle_AnonymousDocumentWithArray_ArrayItemsStored()
        {
            var document2Insert = new { Name = "Daniel", Age = 29, WorkDays = new[] { 1, 1, 1, 0, 1, 0, 0 } };

            using (var cn = TestHelper.CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Documents = new[] { document2Insert }
                                    };
                insertCommand.Execute();
            }

            var inferedTemplate = new { _id = SimoId.Empty, WorkDays = new int[] { } };
            var refetched = TestHelper.GetDocument(document2Insert, inferedTemplate, Constants.Collections.PersonsCollectionName);
            CollectionAssert.AreEqual(document2Insert.WorkDays, refetched.WorkDays);
        }

        [TestMethod]
        public void InsertSingle_JsonDocument_IsStoredAndAssignedId()
        {
            var document2Insert = @"{Name : ""Daniel"", Age : 29}";

            using (var cn = TestHelper.CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Documents = new[] { document2Insert }
                                    };
                insertCommand.Execute();
            }

            var inferedTemplate = new { _id = SimoId.Empty };
            var refetched = TestHelper.GetDocument(document2Insert, inferedTemplate, Constants.Collections.PersonsCollectionName);
            Assert.AreNotEqual(SimoId.Empty, refetched._id);
        }

        [TestMethod]
        public void InsertSingle_TypedDocumentWithNoIdContainer_IsStoredAndAssignedId()
        {
            var person2Insert = new Person { Name = "Daniel", Age = 29 };

            using (var cn = TestHelper.CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Documents = new[] { person2Insert }
                                    };
                insertCommand.Execute();
            }

            var inferedTemplate = new { _id = SimoId.Empty };
            var refetched = TestHelper.GetDocument(person2Insert, inferedTemplate, Constants.Collections.PersonsCollectionName);
            Assert.AreNotEqual(SimoId.Empty, refetched._id);
        }

        [TestMethod]
        public void InsertSingle_TypedDocumentWithAssignedId_IsStored()
        {
            var person2Insert = new PersonWithId { _id = SimoId.NewId(), Name = "Daniel", Age = 29 };

            using (var cn = TestHelper.CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Documents = new[] { person2Insert }
                                    };
                insertCommand.Execute();
            }

            var refetched = TestHelper.GetDocument<PersonWithId>(person2Insert, Constants.Collections.PersonsCollectionName);
            Assert.AreEqual(person2Insert._id, refetched._id);
        }

        [TestMethod, ExpectedException(typeof(SerializationException))]
        public void InsertSingle_TypedDocumentWithEmptyAssignedId_ThrowsException()
        {
            var person2Insert = new PersonWithId { _id = SimoId.Empty, Name = "Daniel", Age = 29 };

            using (var cn = TestHelper.CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Documents = new[] { person2Insert }
                                    };

                insertCommand.Execute();
            }

            Assert.Fail("Should not have been able to insert document with Empty ObjectId.");
        }

        [TestMethod]
        public void InsertSingle_TypedDocumentWithAutoId_IsStoredAndAssignedId()
        {
            var person2Insert = new PersonWithAutoId { Name = "Daniel", Age = 29 };

            using (var cn = TestHelper.CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                        Documents = new[] { person2Insert }
                                    };
                insertCommand.Execute();
            }

            var refetched = TestHelper.GetDocuments<PersonWithId>(new { person2Insert._id }, Constants.Collections.PersonsCollectionName);
            Assert.AreEqual(1, refetched.Count);
        }

        [TestMethod]
        public void InsertSingle_TypedNestedDocument_NestedObjectIsStoredWithNoId()
        {
            var car2Insert = new Car
                             {
                                 RegNo = "ABC123",
                                 Owner = new Owner { Name = "Daniel" }
                             };

            using (var cn = TestHelper.CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                                    {
                                        FullCollectionName = Constants.Collections.CarsFullCollectionName,
                                        Documents = new[] { car2Insert }
                                    };
                insertCommand.Execute();
            }

            var refetched = TestHelper.GetDocument<Car>(new Car { RegNo = car2Insert.RegNo }, Constants.Collections.CarsCollectionName);
            Assert.IsNotNull(refetched.Owner);
            Assert.IsNull(refetched.Owner._id);
        }
    }
}


