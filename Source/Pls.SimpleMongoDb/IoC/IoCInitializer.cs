using System.IO;
using Newtonsoft.Json;
using Pls.SimpleMongoDb.Globalization;
using Pls.SimpleMongoDb.Serialization;
using System;
using System.Collections.Generic;

namespace Pls.SimpleMongoDb.IoC
{
    public static class IoCInitializer
    {
        public static void InitializeIoC(ISimoIoC container)
        {
            container.RegisterNoArgs<ISimoPluralizer>(() => new SimoPluralizer());

            container.RegisterWithArgs<IDocumentWriter>(args =>
            {
                var stream = args.Get<Stream>(0);
                return new DocumentWriter(stream);
            });

            container.RegisterWithArgs<ISelectorWriter>(args =>
            {
                var stream = args.Get<Stream>(0);
                return new SelectorWriter(stream);
            });

            container.RegisterNoArgs<JsonSerializer, IDocumentWriter>(() =>
                new JsonSerializer
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });

            JsonSerializer doconv = new JsonSerializer()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            doconv.Converters.Add(new SimpleIdJsonConverter());
            container.RegisterNoArgs<JsonSerializer, IDocumentReader>(() => doconv);

            JsonSerializer selconv = new JsonSerializer
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                };

            container.RegisterNoArgs<JsonSerializer, ISelectorWriter>(() => selconv);
        }
    }
    internal class SimpleIdJsonConverter
           : JsonConverter
    {
        public SimpleIdJsonConverter() { }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var oid = value as byte[];
            //writer.Path
            writer.WriteValue(Convert.ToBase64String(oid));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }
    }
  
}