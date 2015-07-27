using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    [TestClass]
    public class SimoSessionFactoryTests
    {
        [TestMethod]
        public void GetSession_WithoutArgs_UsesDefaultSimoConnectionInfo()
        {
            var factory = new SimoSessionFactory();

            using(var session = factory.GetSession())
            {
               Assert.AreEqual(SimoConnectionInfo.Default, session.Connection.SimoConnectionInfo);
            }
        }
    }
}