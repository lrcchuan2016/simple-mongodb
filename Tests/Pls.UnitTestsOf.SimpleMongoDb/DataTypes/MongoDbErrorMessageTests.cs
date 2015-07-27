using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.DataTypes;
using Pls.SimpleMongoDb.Resources;

namespace Pls.UnitTestsOf.SimpleMongoDb.DataTypes
{
    [TestClass]
    public class MongoDbErrorMessageTests
    {
        [TestMethod]
        public void Blank_ReturnsInstanceWithEmptyMessage()
        {
            var errMsg = MongoDbErrorMessage.Blank;

            Assert.AreEqual(string.Empty, errMsg.Message);
        }

        [TestMethod]
        public void FromDocument_DocumentHasErrorWithMessage_ReturnsNonNullWithMessage()
        {
            var document = new SimoKeyValues { { "ok", 0.0 }, {"errmsg", "Dummy"} };

            var errMsg = MongoDbErrorMessage.FromDocument(document);

            Assert.AreEqual("Dummy", errMsg.Message);
        }

        [TestMethod]
        public void FromDocument_DocumentHasErrorWithoutMessage_ReturnsNonNullWithDefaultMessage()
        {
            var document = new SimoKeyValues { { "ok", 0.0 } };

            var errMsg = MongoDbErrorMessage.FromDocument(document);

            Assert.AreEqual(ExceptionMessages.MongoErrorMessage_Default, errMsg.Message);
        }

        [TestMethod]
        public void FromDocument_DocumentHasNoError_ReturnsNull()
        {
            var document = new SimoKeyValues();

            var errMsg = MongoDbErrorMessage.FromDocument(document);

            Assert.IsNull(errMsg);
        }
    }
}