using DateOnlyTimeOnly.AspNet.Converters;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SalesManager.Application.Base.Services
{
    public class SerializationService : ISerializationService
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Converters =
            {
                new TimeOnlyJsonConverter(),
                new DateOnlyJsonConverter(),
                new JsonStringEnumConverter()
            },
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
        };

        public byte[] SerializeToArray<T>(T model)
        {
            return JsonSerializer.SerializeToUtf8Bytes(model, Options);
        }

        public T Deserialize<T>(string source)
        {
            return JsonSerializer.Deserialize<T>(source, Options)!;
        }

        public object Deserialize(byte[] value, Type type)
        {
            return JsonSerializer.Deserialize(value, type, Options);
        }

        public T Deserialize<T>(byte[] value)
        {
            return JsonSerializer.Deserialize<T>(value, Options);
        }

        public string Serialize<T>(T model)
        {
            return JsonSerializer.Serialize(model, Options);
        }

        public string Serialize<T>(T model, JsonSerializerOptions options)
        {
            return JsonSerializer.Serialize(model, options);
        }
    }
}
