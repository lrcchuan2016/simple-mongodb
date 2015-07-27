using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    [TestClass]
    public class SimoDatabaseTests
    {
        private const string DbName = Constants.TestDbName;

        [TestMethod]
        public void GetLastError_SingleErrorExists_ReturnsSingleError()
        {
            using(var session = ObjectFactoryForTests.CreateSession())
            {
                var db = session[DbName];
                db.DropDatabase();
                db.DropDatabase();

                var message = db.GetLastError();
            }
        }
    }
}