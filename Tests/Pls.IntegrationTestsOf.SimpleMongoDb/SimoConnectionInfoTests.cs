using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    [TestClass]
    public class SimoConnectionInfoTests
        : TestBase
    {
        [TestMethod]
        public void Ctor_WithAppConfig_UsingConnectionStringName()
        {
            var connectionInfo = new SimoConnectionInfo(Constants.ConnectionStringName);

            Assert.AreEqual(27017, connectionInfo.Port);
            Assert.AreEqual("localhost", connectionInfo.Host);
        }
    }
}