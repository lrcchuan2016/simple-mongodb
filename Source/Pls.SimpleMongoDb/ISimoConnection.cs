using System;
using System.IO;

namespace Pls.SimpleMongoDb
{
    public interface ISimoConnection
        : IDisposable
    {
        ISimoConnectionInfo SimoConnectionInfo { get; }
        bool IsConnected { get; }
        long ConnectionActs { get; }

        bool Connect();
        void Disconnect();
        Stream GetPipeStream();
    }
}   