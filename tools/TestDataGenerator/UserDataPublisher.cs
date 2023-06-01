using System.Text.Json;
using Bogus;
using Confluent.Kafka;

public record User(long Id, string FirstName, string LastName, DateTimeOffset DateOfBirth);

public class UserSerializer : ISerializer<User?> {
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public byte[] Serialize(User? data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data, _options);
    }
}

public class UserDataPublisher : TestDataPublisher
{
    private readonly string _topic;
    private readonly Random _random = new();
    private readonly IProducer<long, User?> _producer;

    public UserDataPublisher(string topic)
    {
        _topic = topic;
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092,localhost:9093,localhost:9094"
        };

        _producer = new ProducerBuilder<long, User?>(producerConfig)
            .SetKeySerializer(Serializers.Int64)
            .SetValueSerializer(new UserSerializer())
            .Build();
    }
    
    public override async Task Run(CancellationToken token)
    {
        long userIdSeed = 0;
        var faker = new Faker<User>()
            .CustomInstantiator(f => new User(
                Id: userIdSeed++, 
                FirstName: f.Name.FirstName(),
                LastName: f.Name.LastName(), 
                DateOfBirth: f.Date.PastOffset(20, DateTimeOffset.Now.AddYears(-20))));

        var initial = faker.Generate(100);

        foreach (var user in initial)
        {
            var msg = new Message<long, User?>
            {
                Key = user.Id,
                Value = user
            };
            
            await _producer.ProduceAsync(_topic, msg, token);
            Console.WriteLine($"Produced {msg.Key}:{msg.Value} to {_topic}");
        }
        
        while (!token.IsCancellationRequested)
        {
            if (_shouldRun)
            {
                var userMsgs = faker.GenerateBetween(1, 3).Select(u => new Message<long, User?>
                {
                    Key = u.Id,
                    Value = u
                });

                foreach (var msg in userMsgs)
                {
                    await _producer.ProduceAsync(_topic, msg, token);
                    Console.WriteLine($"Produced {msg.Key}:{msg.Value} to {_topic}");
                    await Task.Delay(_random.Next(50, 500), token);
                }
            }

            await Task.Delay(_random.Next(1000, 5000), token);
        }
    }
}