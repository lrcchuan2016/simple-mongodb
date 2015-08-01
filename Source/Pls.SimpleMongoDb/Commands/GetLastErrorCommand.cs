namespace Pls.SimpleMongoDb.Commands
{
    public class GetLastErrorCommand
        : DatabaseCommand
    {
        /// <summary>
        /// Defines the Database-name to be queried for
        /// last errors.
        /// </summary>
        public string DatabaseName
        {
            get { return NodeName; }
            set { NodeName = value; }
        }

        public GetLastErrorCommand(ISimoConnection connection, Reconnection OnReconnect)
            : base(connection, OnReconnect, new { getlasterror = 1.0 })
        {
        }
    }
}