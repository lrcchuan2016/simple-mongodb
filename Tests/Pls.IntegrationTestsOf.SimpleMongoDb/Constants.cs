namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    internal static class Constants
    {
        internal const string ConnectionStringName = "SimpleMongoDbTests";
        internal const string TestDbName = "TestDb";

        internal static class Collections
        {
            internal const string PersonsCollectionName = "Persons";
            internal const string CarsCollectionName = "Cars";

            internal const string ParentsCollectionName = "Parents";
            internal const string ChildsCollectionName = "Childs";

            internal const string PersonsFullCollectionName = TestDbName + "." + PersonsCollectionName;
            internal const string CarsFullCollectionName = TestDbName + "." + CarsCollectionName;

            internal const string ParentsFullCollectionName = TestDbName + "." + ParentsCollectionName;
            internal const string ChildsFullCollectionName = TestDbName + "." + ChildsCollectionName;
        }
    }
}


