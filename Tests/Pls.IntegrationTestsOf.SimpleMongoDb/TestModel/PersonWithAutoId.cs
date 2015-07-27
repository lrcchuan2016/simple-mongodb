using System;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.TestModel
{
    [Serializable]
    public class PersonWithAutoId
        : Person
    {
        private SimoAutoId _simoAutoId = new SimoAutoId();

        public SimoId _id
        {
            get { return _simoAutoId; }
            set { _simoAutoId = value; }
        }
    }
}