using System;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.TestModel
{
    [Serializable]
    public class Child
    {
        private readonly SimoAutoId _simoAutoId = new SimoAutoId();

        public SimoId _id
        {
            get { return _simoAutoId; }
        }

        public virtual string Name { get; set; }
        public virtual SimoReference FatherReference { get; set; }
    }
}