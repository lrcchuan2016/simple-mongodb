using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.UnitTestsOf.SimpleMongoDb.DataTypes
{
    [TestClass]
    public class SimoKeyValuesTests
    {
        [TestMethod]
        public void GetString_WhenValueIsPresent_ReturnsString()
        {
            var keyValues = CreateKeyValues("TheString", "A");

            keyValues.AssertValue("TheString", "A", keyValues.GetString);
        }

        [TestMethod]
        public void GetString_WhenNullValue_ReturnsNull()
        {
            var keyValues = CreateKeyValues<string>("TheString", null);

            keyValues.AssertValue("TheString", null, keyValues.GetString);
        }

        [TestMethod]
        public void GetDouble_WhenValueIsPresent_ReturnsDobule()
        {
            var keyValues = CreateKeyValues("TheString", 1.5);

            keyValues.AssertValue("TheString", 1.5, keyValues.GetDouble);
        }

        [TestMethod]
        public void GetDouble_WhenNullValue_ReturnsNull()
        {
            var keyValues = CreateKeyValues<double?>("TheString", null);

            keyValues.AssertValue("TheString", null, keyValues.GetDouble);
        }

        private static SimoKeyValues CreateKeyValues<T>(string key, T value)
        {
            return new SimoKeyValues { { key, value } };
        }
    }
}