namespace Pls.SimpleMongoDb.Commands
{
    public class AuthenticateCommand
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
        /// <summary>
        /// http://docs.mongodb.org/meta-driver/latest/legacy/implement-authentication-in-driver/
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="UserName"></param>
        /// <param name="KeyDigest"></param>
        /// <param name="nonce"></param>
        public AuthenticateCommand(ISimoConnection connection, string UserName, string KeyDigest, string nonce)
            : base(connection, new { 
                authenticate = 1.0, 
                user = UserName, 
                nonce = nonce, 
                key = KeyDigest })
        {
        }
    }
}