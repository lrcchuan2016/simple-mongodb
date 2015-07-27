using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb;

namespace Pls.UnitTestsOf.SimpleMongoDb
{
    [TestClass]
    public class SimoHelperTests
    {
        [TestMethod]
        public void GetDatabaseName_FullCollectionName_ReturnsDatabaseName()
        {
            var nodeName = "TestDb.Persons";

            var dbName = SimoHelper.GetDatabaseName(nodeName);

            Assert.AreEqual("TestDb", dbName);
        }

        [TestMethod]
        public void GetDatabaseName_DatabaseName_ReturnsDatabaseName()
        {
            var nodeName = "TestDb";

            var dbName = SimoHelper.GetDatabaseName(nodeName);

            Assert.AreEqual("TestDb", dbName);
        }
    }
}