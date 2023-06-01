using Confluent.Kafka;
using KafkaWorkshop.Solutions.Shared.Contracts;
using KafkaWorkshop.Solutions.Shared.Serde;
using Microsoft.Extensions.Options;

namespace KafkaWorkshop.Solutions.UsersApi;

public class UsersConsumerService : BackgroundService
{
    private readonly UsersDb _usersDb;
    private readonly UsersDbLoaderMonitor _usersDbLoaderMonitor;
    private readonly KafkaOptions _kafkaOptions;
    private readonly IConsumer<long ,User?> _consumer;

    public UsersConsumerService(
        UsersDb usersDb, 
        UsersDbLoaderMonitor usersDbLoaderMonitor, 
        IOptions<KafkaOptions> kafkaSettings)
    {
        _usersDb = usersDb;
        _usersDbLoaderMonitor = usersDbLoaderMonitor;
        _kafkaOptions = kafkaSettings.Value;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServers,
            GroupId = Guid.NewGuid().ToString(),
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnablePartitionEof = true
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
                ConsumeResult<long, User?> incomingMessage = _consumer.Consume(stoppingToken);

                if (incomingMessage.IsPartitionEOF)
                {
                    _usersDbLoaderMonitor.SignalLoaded();
                }
                else if (incomingMessage.Message.Value is null)
                {
                    _usersDb.Remove(incomingMessage.Message.Key);
                }
                else
                {
                    _usersDb.AddOrUpdate(incomingMessage.Message.Value);
                }
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