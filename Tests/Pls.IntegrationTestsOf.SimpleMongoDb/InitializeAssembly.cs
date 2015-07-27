using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    [TestClass]
    public class InitializeAssembly
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            TestHelper.DropTestDatabase();
        }
    }
}