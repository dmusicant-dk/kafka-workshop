using Confluent.Kafka;
using KafkaWorkshop.Solutions.Shared.Contracts;
using KafkaWorkshop.Solutions.Shared.Serde;

const string Topic = "kafka-exercise-producer-2";

var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092,localhost:9093,localhost:9094"
};

var producer = new ProducerBuilder<long, User?>(producerConfig)
    .SetKeySerializer(Serializers.Int64)
    .SetValueSerializer(new UserSerializer())
    .Build();

var users = new List<User>
{
    new(1, "John", "Doe", new DateTimeOffset(1980, 1, 1, 0, 0, 0, TimeSpan.Zero)),
    new(2, "Jane", "Doe", new DateTimeOffset(1981, 1, 1, 0, 0, 0, TimeSpan.Zero)),
    new(3, "John", "Smith", new DateTimeOffset(1982, 1, 1, 0, 0, 0, TimeSpan.Zero)),
    new(4, "Jane", "Smith", new DateTimeOffset(1983, 1, 1, 0, 0, 0, TimeSpan.Zero)),
};

foreach (var user in users)
{
    var message = new Message<long, User?>
    {
        Key = user.Id,
        Value = user
    };
    producer.Produce(Topic, message, report =>
    {
        Console.WriteLine("Produced message to topic {0} partition {1} @ offset {2}",
            report.TopicPartition.Topic,
            report.TopicPartition.Partition,
            report.Offset);
    });
}

Console.ReadKey(true);