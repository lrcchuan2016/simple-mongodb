using System.Collections.Generic;

namespace Pls.SimpleMongoDb
{
    public interface ISimoDatabase
    {
        ISimoSession Session { get; }
        string Name { get; }
        void Autorise(string user, string pwd);
        void AutoriseOnLostConnection();

        ISimoCollection this[string name] { get; }
        bool Authorised { get; }
        void DropDatabase();
        void DropCollections(params string[] collectionNames);

        ISimoCollection GetCollection<T>() where T : class;
        IList<string> GetCollectionNames(bool includeSystemCollections = false);
    }
}