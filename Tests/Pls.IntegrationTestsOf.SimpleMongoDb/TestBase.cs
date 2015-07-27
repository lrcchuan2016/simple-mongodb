using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    [TestClass]
    public abstract class TestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            //TestHelper.DropTestDatabase();
            TestHelper.DropCollections();
            OnTestInitialize();
        }

        protected virtual void OnTestInitialize()
        { }

        [TestCleanup]
        public void TestCleanup()
        {
            OnTestCleanup();
        }

        protected virtual void OnTestCleanup()
        {
        }
    }
}