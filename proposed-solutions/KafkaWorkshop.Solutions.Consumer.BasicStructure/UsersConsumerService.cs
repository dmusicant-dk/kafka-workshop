using Confluent.Kafka;
using KafkaWorkshop.Solutions.Shared.Contracts;
using KafkaWorkshop.Solutions.Shared.Serde;
using Microsoft.Extensions.Options;

namespace KafkaWorkshop.Solutions.Consumer.BasicStructure;

public class UsersConsumerService : BackgroundService
{
    private readonly KafkaOptions _kafkaOptions;
    private readonly IConsumer<long ,User?> _consumer;

    public UsersConsumerService(
        IOptions<KafkaOptions> kafkaSettings)
    {
        _kafkaOptions = kafkaSettings.Value;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServers,
            GroupId = Guid.NewGuid().ToString(),
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        _consumer = new ConsumerBuilder<long, User?>(consumerConfig)
            .SetKeyDeserializer(Deserializers.Int64)
            .SetValueDeserializer(new UserDeserializer())
            .Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            _consumer.Subscribe(_kafkaOptions.Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<long, User?> incomingMessage = _consumer.Consume(); // message wil be never null!

                if (incomingMessage.Message.Value is null) {
                    Console.WriteLine($"Consumed record with key: {incomingMessage.Message.Key} and NULL value");
                    continue;
                }
	
                Console.WriteLine($"Consumed record with key: {incomingMessage.Message.Key} and firstName: {incomingMessage.Message.Value.FirstName}");
            }
        }, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        
        _consumer.Close();
        _consumer.Dispose();
    }
}