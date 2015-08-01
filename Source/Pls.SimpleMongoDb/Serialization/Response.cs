using System;
using System.Collections.Generic;
using Pls.SimpleMongoDb.Commands;

namespace Pls.SimpleMongoDb.Serialization
{
    public enum eResponseFlag : int
    {
        undefined = -1,
        CursorNotFound = 0,//0
        QueryFailure,//1
        ShardConfigStale,//2
        AwaitCapable,//3
        Reserved//4-31
    }
    /// <summary>
    /// Represents a response that is returned by the server.
    /// All commands does not generate a response.
    /// </summary>
    /// <seealso cref="http://www.mongodb.org/display/DOCS/Mongo+Wire+Protocol#MongoWireProtocol-StandardMessageHeader"/>
    /// <seealso cref="http://www.mongodb.org/display/DOCS/Mongo+Wire+Protocol#MongoWireProtocol-OPREPLY"/>
    [Serializable]
    public class Response<TDocument>
    {
        private readonly List<TDocument> _returnedDocuments;

        /// <summary>
        /// The Total length of the Response-bytes.
        /// </summary>
        public int? TotalLength { get; set; }

        /// <summary>
        /// Client or database-generated identifier for this Response.
        /// </summary>
        public int? RequestId { get; set; }

        /// <summary>
        /// RequestID from the original request (used in reponses from db)
        /// </summary>
        public int? ResponseTo { get; set; }

        /// <summary>
        /// Request type. <see cref="OpCodes"/> for possible codes.
        /// </summary>
        public OpCodes? OpCode { get; set; }

        public int? ResponseFlags { get; set; }

        public eResponseFlag ResponseFlag
        {
            get
            {
                if (!ResponseFlags.HasValue)
                    return eResponseFlag.undefined;

                string s = Convert.ToString(ResponseFlags.Value, 2);
                if (BitConverter.IsLittleEndian)
                {
                    char[] charArray = s.ToCharArray();
                    Array.Reverse(charArray);
                    s = new string(charArray);
                }
                eResponseFlag result = eResponseFlag.undefined;

                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '1')
                    {
                        result = (eResponseFlag)i;
                        break;
                    }
                }

                return result;
            }
        }
        public long? CursorId { get; set; }

        public int? StartingFrom { get; set; }

        public int? NumberOfReturnedDocuments { get; set; }

        public bool CursorExists
        {
            get { return CursorId.HasValue && CursorId.Value > 0; }
        }

        /// <summary>
        /// Returned documents.
        /// </summary>
        public IList<TDocument> ReturnedDocuments
        {
            get { return _returnedDocuments; }
        }

        public Response()
        {
            _returnedDocuments = new List<TDocument>();
        }

        public void SetDocuments(IEnumerable<TDocument> documents)
        {
            _returnedDocuments.Clear();
            _returnedDocuments.AddRange(documents);
        }

    }
}