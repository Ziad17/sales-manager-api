using System.Text.Json;

namespace SalesManager.Application.Base.Services
{
    public interface ISerializationService
    {
        string Serialize<T>(T model);

        string Serialize<T>(T model, JsonSerializerOptions options);

        byte[] SerializeToArray<T>(T model);

        T Deserialize<T>(string source);

        object Deserialize(byte[] value, Type type);

        T Deserialize<T>(byte[] value);
    }
}
