using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.DataTypes;
using Pls.SimpleMongoDb.Exceptions;

namespace Pls.UnitTestsOf.SimpleMongoDb.DataTypes
{
    [TestClass]
    public class SimoObjectIdTests
    {
        [TestMethod]
        public void Empty_Value_Contains12EmptyBytes()
        {
            var id = SimoId.Empty;

            CollectionAssert.AreEqual(new byte[12], id.Value);
        }

        [TestMethod, ExpectedException(typeof(SimoException))]
        public void Ctor_NullBytes_ThrowsException()
        {
            var id = new SimoId(null as byte[]);
        }

        [TestMethod, ExpectedException(typeof(SimoException))]
        public void Ctor_EmptyBytes_ThrowsException()
        {
            var id = new SimoId(new byte[11]);
        }

        [TestMethod, ExpectedException(typeof(SimoException))]
        public void Ctor_ToFewBytes_ThrowsException()
        {
            var id = new SimoId(new byte[11]);
        }

        [TestMethod, ExpectedException(typeof(SimoException))]
        public void Ctor_ToManyBytes_ThrowsException()
        {
            var id = new SimoId(new byte[13]);
        }

        [TestMethod]
        public void NewId_NotEqualToEmptyObjectId()
        {
            var id = SimoId.NewId();

            Assert.AreNotEqual(SimoId.Empty, id);
        }

        [TestMethod]
        public void NewId_GetTwo_NotEqualToEachOther()
        {
            var id1 = SimoId.NewId();
            var id2 = SimoId.NewId();

            Assert.AreNotEqual(id1, id2);
        }

        [TestMethod]
        public void TimeStamp_NewId_HasTodaysUtcDate()
        {
            //TODO: Isolate date
            var id = SimoId.NewId();

            Assert.AreEqual(DateTime.UtcNow.Date, id.TimeStamp.Date);
        }

        [TestMethod]
        public void IsNullOrEmpty_ForEmpty_IsTrue()
        {
            var id = SimoId.Empty;

            Assert.IsTrue(SimoId.IsNullOrEmpty(id));
        }

        [TestMethod]
        public void IsNullOrEmpty_ForNull_IsTrue()
        {
            Assert.IsTrue(SimoId.IsNullOrEmpty(null));
        }
    }
}