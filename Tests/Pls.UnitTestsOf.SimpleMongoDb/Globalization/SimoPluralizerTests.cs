using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Globalization;

namespace Pls.UnitTestsOf.SimpleMongoDb.Globalization
{
    [TestClass]
    public class SimoPluralizerTests
    {
        [TestMethod]
        public void Pluralize_WhenDisabled_SinglurarizedIsPluralized()
        {
            var singularized = "Child";
            var pluralizer = new SimoPluralizer();
            pluralizer.Disable();

            var nonPluralized = pluralizer.Pluralize(singularized);

            Assert.AreEqual(singularized, nonPluralized);
        }

        [TestMethod]
        public void Pluralize_WhenEnabled_SingularizedIsPluralized()
        {
            var pluralizer = new SimoPluralizer();

            var nonPluralized = pluralizer.Pluralize("Car");

            Assert.AreEqual("Cars", nonPluralized);
        }

        [TestMethod]
        public void Pluralize_DisabledValues_SpecificPluralization()
        {
            var pluralizer = new SimoPluralizer();

            Assert.AreEqual("Persons", pluralizer.Pluralize("Person"));
            Assert.AreEqual("Childs", pluralizer.Pluralize("Child"));
        }

        [TestMethod]
        public void ClearPluralizationAdjustments_PreviouslyAdjustedValue_IsPluralized()
        {
            var pluralizer = new SimoPluralizer();
            
            pluralizer.ClearPluralizationAdjustments();

            Assert.AreEqual("People", pluralizer.Pluralize("Person"));
        }

        [TestMethod]
        public void AdjustPluralization_IsCustomPluralized()
        {
            var pluralizer = new SimoPluralizer();

            pluralizer.AdjustPluralization("Monkey", "Test");

            Assert.AreEqual("Test", pluralizer.Pluralize("Monkey"));
        }
    }
}