using System;
using Pls.SimpleMongoDb.DataTypes;
using Pls.SimpleMongoDb.Exceptions;

namespace Pls.SimpleMongoDb.Commands
{
    [Serializable]
    public class SimoCommandException
        : SimoException
    {
        public MongoDbErrorMessage MongoDbDbMessage { get; private set; }

        public SimoCommandException(string message, MongoDbErrorMessage mongoDbDbMessage = null)
            : base(message)
        {
            MongoDbDbMessage = mongoDbDbMessage ?? MongoDbErrorMessage.Blank;
        }
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}