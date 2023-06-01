namespace KafkaWorkshop.Solutions.Shared.Serde;

using System.Text.Json;
using Confluent.Kafka;

using Contracts;

public class UserDeserializer : IDeserializer<User?>
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public User? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) => 
        isNull ? null : JsonSerializer.Deserialize<User>(data, _options);
}