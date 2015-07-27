using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Querying;

namespace Pls.UnitTestsOf.SimpleMongoDb.Querying
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod]
        public void In_SeveralStringOperands_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Name"].In("Daniel", "Sue"));
            
            Assert.AreEqual(@"{ Name: { $in: [""Daniel"",""Sue""] } }", query);
        }

        [TestMethod]
        public void NotIn_SeveralStringOperands_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Name"].NotIn("Daniel", "Sue"));

            Assert.AreEqual(@"{ Name: { $nin: [""Daniel"",""Sue""] } }", query);
        }

        [TestMethod]
        public void In_SeveralIntOperands_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Name"].In(21, 22));

            Assert.AreEqual(@"{ Name: { $in: [21,22] } }", query);
        }

        [TestMethod]
        public void InAndIn_WithStringsAndInts_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Name"].In("Daniel", "Sue").And("Age").In(21, 22));

            Assert.AreEqual(@"{ Name: { $in: [""Daniel"",""Sue""] }, Age: { $in: [21,22] } }", query);
        }

        [TestMethod]
        public void Where_WithStringAndInt_BuildsCorrectFormat()
        {
            var query = Query.New(q => q.Where(@"this.Name == 'Daniel' || this.Age == 21"));

            Assert.AreEqual(@"{ $where: "" this.Name == 'Daniel' || this.Age == 21 "" }", query);
        }

        [TestMethod]
        public void InAndWhere_InFirstAndWhereSecond_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Name"].In("Daniel", "Sue").And().Where(@"this.Age == 21 || this.Age == 22"));

            Assert.AreEqual(@"{ Name: { $in: [""Daniel"",""Sue""] }, $where: "" this.Age == 21 || this.Age == 22 "" }", query);
        }

        [TestMethod]
        public void InAndWhere_WhereFirstAndInSecond_BuildsCorrectFormat()
        {
            var query = Query.New(q => q.Where(@"this.Age == 21 || this.Age == 22").And("Name").In("Daniel", "Sue"));

            Assert.AreEqual(@"{ $where: "" this.Age == 21 || this.Age == 22 "", Name: { $in: [""Daniel"",""Sue""] } }", query);
        }

        [TestMethod]
        public void Lt_WhitInt_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].Lt(50));

            Assert.AreEqual(@"{ Age: { $lt: 50 } }", query);
        }

        [TestMethod]
        public void LtE_WhitInt_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].LtEq(50));

            Assert.AreEqual(@"{ Age: { $lte: 50 } }", query);
        }

        [TestMethod]
        public void Gt_WhitInt_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].Gt(50));

            Assert.AreEqual(@"{ Age: { $gt: 50 } }", query);
        }

        [TestMethod]
        public void GtE_WhitInt_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].GtEq(50));

            Assert.AreEqual(@"{ Age: { $gte: 50 } }", query);
        }

        [TestMethod]
        public void InAndLt_ChainedOnSameProperty_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].In(21, 22, 23).Lt(23));
            
            Assert.AreEqual(@"{ Age: { $in: [21,22,23], $lt: 23 } }", query);
        }

        [TestMethod]
        public void Between_WithInts_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].Between(21, 29));

            Assert.AreEqual(@"{ Age: { $gte: 21, $lte: 29 } }", query);
        }

        [TestMethod]
        public void NotEqual_WithInts_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].NotEq(21));

            Assert.AreEqual(@"{ Age: { $ne: 21 } }", query);
        }

        [TestMethod]
        public void NotEqual_WithStrings_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Name"].NotEq("Daniel"));

            Assert.AreEqual(@"{ Name: { $ne: ""Daniel"" } }", query);
        }

        [TestMethod]
        public void Mod_WithInts_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].Mod(2, 0));

            Assert.AreEqual(@"{ Age: { $mod: [2,0] } }", query);
        }

        [TestMethod]
        public void All_WithInts_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Workdays"].HasAll(1, 2, 3, 4, 5));

            Assert.AreEqual(@"{ Workdays: { $all: [1,2,3,4,5] } }", query);
        }

        [TestMethod]
        public void All_WithStrings_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Workdays"].HasAll("Mon", "Tue", "Wed", "Thu", "Fri"));

            Assert.AreEqual(@"{ Workdays: { $all: [""Mon"",""Tue"",""Wed"",""Thu"",""Fri""] } }", query);
        }

        [TestMethod]
        public void Size_WithStringArray_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Workdays"].Size(5));

            Assert.AreEqual(@"{ Workdays: { $size: 5 } }", query);
        }

        [TestMethod]
        public void Exists_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].Exists());

            Assert.AreEqual(@"{ Age: { $exists: true } }", query);
        }

        [TestMethod]
        public void NotExists_BuildsCorrectFormat()
        {
            var query = Query.New(q => q["Age"].NotExists());

            Assert.AreEqual(@"{ Age: { $exists: false } }", query);
        }
    }
}