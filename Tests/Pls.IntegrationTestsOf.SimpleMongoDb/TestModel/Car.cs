using System;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.TestModel
{
    [Serializable]
    public class Car
    {
        public string RegNo { get; set; }
        public Owner Owner { get; set; }
    }
}