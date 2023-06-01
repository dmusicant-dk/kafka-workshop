using Confluent.Kafka;
using KafkaWorkshop.Solutions.Shared.Contracts;
using KafkaWorkshop.Solutions.Shared.Serde;
using Microsoft.Extensions.Options;

namespace KafkaWorkshop.Solutions.UsersApi;

public class UsersProducer
{
    private readonly KafkaOptions _kafkaOptions;
    private readonly IProducer<long,User?> _producer;

    public UsersProducer(IOptions<KafkaOptions> kafkaOptions)
    {
        _kafkaOptions = kafkaOptions.Value;
        
        _producer = new ProducerBuilder<long, User?>(new ProducerConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers
            })
            .SetKeySerializer(Serializers.Int64)
            .SetValueSerializer(new UserSerializer())
            .Build();
    }

    public async Task ProduceUpdate(User user)
    {
        await _producer.ProduceAsync(_kafkaOptions.Topic, new Message<long, User?>
        {
            Key = user.Id,
            Value = user
        });
    }
    
    public async Task ProduceDelete(long userId)
    {
        await _producer.ProduceAsync(_kafkaOptions.Topic, new Message<long, User?>
        {
            Key = userId,
            Value = null
        });
    }
}