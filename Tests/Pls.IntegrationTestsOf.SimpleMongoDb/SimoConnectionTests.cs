using System;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb;
using Pls.SimpleMongoDb.Exceptions;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    [TestClass]
    public class SimoConnectionTests
        : TestBase
    {
        private ISimoConnection _connection;

        protected override void OnTestCleanup()
        {
            if (_connection != null && _connection.IsConnected)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        [TestMethod]
        public void Connect_ServerIsUp_CanConnect()
        {
            _connection = TestHelper.CreateConnection();

            _connection.Connect();

            Assert.IsTrue(_connection.IsConnected);
        }

        [TestMethod, ExpectedException(typeof(SocketException))]
        public void Connect_NoServerIsUp_ThrowsException()
        {
            var dummyHost = Guid.NewGuid().ToString();
            var dummyConnectionInfo = new SimoConnectionInfo(dummyHost, SimoConnectionInfo.DefaultPort);
            _connection = new SimoConnection(dummyConnectionInfo);

            _connection.Connect();

            Assert.Fail("Connect should have generated exception.");
        }

        [TestMethod, ExpectedException(typeof(SimoCommunicationException))]
        public void Connect_WhileConnected_ThrowsException()
        {
            _connection = TestHelper.CreateConnection();
            _connection.Connect();

            _connection.Connect();

            Assert.IsTrue(_connection.IsConnected);
        }

        [TestMethod]
        public void Disconnect_WhenConnected_GetsDisconnected()
        {
            _connection = TestHelper.CreateConnection();
            _connection.Connect();

            _connection.Disconnect();

            Assert.IsFalse(_connection.IsConnected);
        }

        [TestMethod]
        public void Dispose_WhenConnected_GetsDisconnected()
        {
            _connection = TestHelper.CreateConnection();
            _connection.Connect();

            _connection.Dispose();

            Assert.IsFalse(_connection.IsConnected);
        }

        [TestMethod]
        public void GetPipeStream_WhenConnected_ReturnsStream()
        {
            _connection = TestHelper.CreateConnection();
            _connection.Connect();

            using(var stream = _connection.GetPipeStream())
            {
                Assert.IsNotNull(stream);
            }
        }

        [TestMethod, ExpectedException(typeof(SimoCommunicationException))]
        public void GetPipeStream_WhileNotConnected_ThrowsException()
        {
            _connection = TestHelper.CreateConnection();

            var stream = _connection.GetPipeStream();

            Assert.IsNull(stream, "Pipestream shall not be returned when not connected!");
        }
    }
}