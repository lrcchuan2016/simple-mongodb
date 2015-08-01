namespace Pls.SimpleMongoDb.Commands
{
    public class DropDatabaseCommand
        : DatabaseCommand
    {
        /// <summary>
        /// Defines the Database-name to be dropped.
        /// </summary>
        public string DatabaseName
        {
            get { return NodeName; }
            set { NodeName = value; }
        }

        public DropDatabaseCommand(ISimoConnection connection, Reconnection OnReconnect)
            : base(connection, OnReconnect, new { dropDatabase = 1.0 })
        {
        }
    }
}