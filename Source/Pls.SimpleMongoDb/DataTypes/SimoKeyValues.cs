using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pls.SimpleMongoDb.DataTypes
{
    [Serializable]
    public class SimoKeyValues
        : Dictionary<string, object>
    {
        public SimoKeyValues()
        {

        }
        protected SimoKeyValues(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
        public string GetString(string key)
        {
            return this[key] as string;
        }

        public double? GetDouble(string key)
        {
            return (double?)this[key];
        }
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}