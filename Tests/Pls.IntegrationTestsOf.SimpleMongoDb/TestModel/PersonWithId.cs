using System;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.TestModel
{
    [Serializable]
    public class PersonWithId
        : Person
    {
        public SimoId _id { get; set; }
    }
}