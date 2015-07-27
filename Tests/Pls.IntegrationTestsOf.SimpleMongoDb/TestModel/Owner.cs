using System;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.TestModel
{
    [Serializable]
    public class Owner
    {
        public SimoId _id { get; set; }
        public string Name { get; set; }
    }
}