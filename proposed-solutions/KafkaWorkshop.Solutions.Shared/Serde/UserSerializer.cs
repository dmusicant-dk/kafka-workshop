using System.Text.Json;
using Confluent.Kafka;
using KafkaWorkshop.Solutions.Shared.Contracts;

namespace KafkaWorkshop.Solutions.Shared.Serde;

public class UserSerializer : ISerializer<User?>
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public byte[]? Serialize(User? data, SerializationContext context) => 
        data != null ? JsonSerializer.SerializeToUtf8Bytes(data, _options) : null;
}