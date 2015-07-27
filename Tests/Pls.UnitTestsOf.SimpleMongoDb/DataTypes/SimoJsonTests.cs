using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.UnitTestsOf.SimpleMongoDb.DataTypes
{
    [TestClass]
    public class SimoJsonTests
    {
        [TestMethod]
        public void SimoJson_ImplicitFromJsonString_EqualsJsonValue()
        {
            var jsonString = @"{ Name : ""Daniel""}";

            SimoJson json = jsonString;

            Assert.AreEqual(jsonString, json.Json);
        }

        [TestMethod]
        public void JsonString_ImplicitFromJson_EqualsJsonValue()
        {
            var jsonString = @"{ Name : ""Daniel""}";
            var json = new SimoJson(jsonString);

            string resultingString = json;

            Assert.AreEqual(jsonString, resultingString);
        }

        [TestMethod]
        public void ToKeyValue_String_Equals()
        {
            var json = new SimoJson(@"{ Name : ""Daniel""}");

            Assert.AreEqual("Daniel", json.ToKeyValue()["Name"]);
        }

        [TestMethod]
        public void ToKeyValue_Int_Equals()
        {
            var json = new SimoJson(@"{ Age : 29 }");

            Assert.AreEqual(29, Convert.ToInt32(json.ToKeyValue()["Age"]));
        }

        [TestMethod]
        public void ToKeyValue_DateTime_Equals()
        {
            var json = new SimoJson(@"{TimeStamp : ""\/Date(1262350073000+0100)\/""}");

            Assert.AreEqual(new DateTime(2010, 1, 1, 13, 47, 53), json.ToKeyValue()["TimeStamp"]);
        }

        [TestMethod]
        public void ToKeyValue_WithManyProps_AllAreMapped()
        {
            var json = new SimoJson(@"{ Name : ""Daniel"", Age : 29, TimeStamp : ""\/Date(1262350073000+0100)\/""}");

            Assert.AreEqual(3, json.ToKeyValue().Count);
        }
    }
}


