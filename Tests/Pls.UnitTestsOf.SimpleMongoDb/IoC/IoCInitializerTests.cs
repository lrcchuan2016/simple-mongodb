using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Globalization;
using Pls.SimpleMongoDb.IoC;
using Pls.SimpleMongoDb.Serialization;

namespace Pls.UnitTestsOf.SimpleMongoDb.IoC
{
    [TestClass]
    public class IoCInitializerTests
    {
        private SimoIoC _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new SimoIoC();
            IoCInitializer.InitializeIoC(_container);
        }

        [TestMethod]
        public void Resolve_ISimoPluralizer_IsSimoPluralizer()
        {
            var pluralizer = _container.Resolve<ISimoPluralizer>();

            Assert.IsInstanceOfType(pluralizer, typeof(SimoPluralizer));
        }

        [TestMethod]
        public void Resolve_IDocumentWriter_IsDocumentWriter()
        {
            using(var stream = new MemoryStream())
            {
                var writer = _container.Resolve<IDocumentWriter>(new[] { stream });

                Assert.IsInstanceOfType(writer, typeof(DocumentWriter));
            }
        }

        [TestMethod]
        public void Resolve_ISelectorWriter_IsSelectorWriter()
        {
            using (var stream = new MemoryStream())
            {
                var writer = _container.Resolve<ISelectorWriter>(new[] { stream });

                Assert.IsInstanceOfType(writer, typeof(SelectorWriter));
            }
        }
    }
}


