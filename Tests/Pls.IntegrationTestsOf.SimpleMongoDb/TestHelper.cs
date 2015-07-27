using System.Collections.Generic;
using System.Linq;
using Pls.SimpleMongoDb;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    internal static class TestHelper
    {
        internal static void DropTestDatabase()
        {
            using (var session = CreateSession())
            {
                var db = session[Constants.TestDbName];
                db.DropDatabase();
            }
        }

        internal static void DropCollections(params string[] collectionNames)
        {
            using (var session = CreateSession())
            {
                var db = session[Constants.TestDbName];
                db.DropCollections(collectionNames);
            }
        }

        internal static void InsertDocument<TDocument>(string fullCollectionName, TDocument document)
            where TDocument : class
        {
            InsertDocuments(fullCollectionName, new[] { document });
        }

        internal static void InsertDocuments<TDocument>(string fullCollectionName, IEnumerable<TDocument> documents)
            where TDocument : class
        {
            using (var session = CreateSession())
            {
                session[Constants.TestDbName][fullCollectionName].Insert(documents);
            }
        }

        internal static int GetDocumentCount(string fullCollectionName)
        {
            return GetDocumentCount(null, fullCollectionName);
        }

        internal static int GetDocumentCount(object selector, string fullCollectionName)
        {
            using (var session = CreateSession())
            {
                return session[Constants.TestDbName][fullCollectionName].Count(selector);
            }
        }

        internal static TDocument GetDocument<TDocument>(object selector, TDocument inferedTemplate, string fullCollectionName)
            where TDocument : class
        {
            using (var session = CreateSession())
            {
                return session[Constants.TestDbName][fullCollectionName].FindOneInfered(inferedTemplate, selector);
            }
        }

        internal static TDocument GetDocument<TDocument>(object selector, string fullCollectionName)
            where TDocument : class
        {
            return GetDocuments<TDocument>(selector, fullCollectionName).SingleOrDefault();
        }

        internal static IList<TDocument> GetDocuments<TDocument>(string fullCollectionName)
            where TDocument : class
        {
            return GetDocuments<TDocument>(null, fullCollectionName);
        }

        internal static IList<TDocument> GetDocuments<TDocument>(object selector, string fullCollectionName)
            where TDocument : class
        {
            using (var session = CreateSession())
            {
                return session[Constants.TestDbName][fullCollectionName].Find<TDocument>(selector);
            }
        }

        internal static ISimoSession CreateSession()
        {
            var cn = CreateConnection();

            return new SimoSession(cn);
        }

        internal static ISimoConnection CreateConnection()
        {
            return new SimoConnection(new SimoConnectionInfo(Constants.ConnectionStringName));
        }
    }
}