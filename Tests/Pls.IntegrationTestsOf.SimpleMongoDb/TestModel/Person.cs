using System;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.TestModel
{
    [Serializable]
    public class Person
    {
        public DateTime TempDate { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int[] WorkDays { get; set; }
        public int[] TimeCodes { get; set; }
        public string[] Tags { get; set; }
    }
}