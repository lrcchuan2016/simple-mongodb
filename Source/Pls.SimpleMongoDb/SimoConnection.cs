using System;
using System.IO;
using System.Net.Sockets;
using Pls.SimpleMongoDb.Exceptions;
using Pls.SimpleMongoDb.Resources;

namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Used to establish connection to a MongoDB-server
    /// by communication using TCP-sockets.
    /// </summary>
    public class SimoConnection
        : ISimoConnection
    {
        private readonly object _lockSocket = new object();

        private TcpClient _socket;

        public ISimoConnectionInfo SimoConnectionInfo { get; private set; }
        
        DateTime socketOpenTime;
        //http://docs.mongodb.org/manual/faq/diagnostics/#does-tcp-keepalive-time-affect-mongodb-deployments
        // ~5minutes
        const int maxkeepAlive = 290;
        public bool IsConnected
        {
            get
            {
                TimeSpan tsl = DateTime.Now - socketOpenTime;
                if (tsl.TotalSeconds > maxkeepAlive)// this will prevent many errors
                {
                    Disconnect();// anyway mongodb will disconnect us
                    return false;
                }

                return _socket != null && _socket.Connected;
            }
        }
        public long ConnectionActs { get; private set; }

        public SimoConnection(ISimoConnectionInfo simoConnectionInfo)
        {
            if (simoConnectionInfo == null)
                throw new ArgumentNullException("simoConnectionInfo");

            SimoConnectionInfo = simoConnectionInfo;
        }

        #region Object lifetime, Disposing

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// A call to Dispose(false) should only clean up native resources.
        /// A call to Dispose(true) should clean up both managed and native resources.
        /// </summary>
        /// <param name="disposeManagedResources"></param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
                DisposeManagedResources();
        }

        ~SimoConnection()
        {
            Dispose(false);
        }

        protected virtual void DisposeManagedResources()
        {
            Disconnect();
        }

        #endregion

        public bool Connect()
        {
            lock (_lockSocket)
            {
                bool recon = ConnectionActs > 0;

                if (_socket != null && _socket.Connected)
                    throw new SimoCommunicationException(ExceptionMessages.MongoConnection_AllreadyEstablished);

                _socket = new TcpClient();
                _socket.Connect(SimoConnectionInfo.Host, SimoConnectionInfo.Port);
 
                DNDS = _socket.GetStream();

                socketOpenTime = DateTime.Now;
                ConnectionActs++;

                return recon;
            }
        }

        public void Disconnect()
        {
            lock (_lockSocket)
            {
                if (_socket != null)
                {
                    if (_socket.Connected)
                        _socket.Close();

                    _socket = null;
                }
            }
        }
        Stream DNDS = null;

        public Stream GetPipeStream()
        {
            if (!IsConnected)
                throw new SimoCommunicationException(ExceptionMessages.MongoConnection_NoPipestreamWhileDisconnected);


            return DNDS;
        }
    }
}
