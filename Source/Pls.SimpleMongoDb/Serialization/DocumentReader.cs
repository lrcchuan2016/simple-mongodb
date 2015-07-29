using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Pls.SimpleMongoDb.Serialization
{
    public class DocumentReader : IDocumentReader
    {
        private readonly Stream _documentStream;
        private readonly JsonSerializer _jsonSerializer;

        public DocumentReader(Stream documentStream)
        {
            _documentStream = documentStream;
            _jsonSerializer = SimoEngine.Instance.IoC.Resolve<JsonSerializer, IDocumentReader>();
        }

        public T ReadDocument<T>()
            where T : class
        {
            T document;

            //using (var reader = new BsonReader(_documentStream))
            //{
            var reader = new BsonReader(_documentStream);
    
            if (typeof(T) == typeof(string))
            {
                object x = _jsonSerializer.Deserialize(reader);
                StringWriter sw = new StringWriter();
                _jsonSerializer.Serialize(sw, x);
                document = sw.ToString() as T;
                //document = JsonConvert.SerializeObject(x) as T;
            }
            else
            {
                document = _jsonSerializer.Deserialize<T>(reader);
            }
            //}

            return document;
        }
    }
}