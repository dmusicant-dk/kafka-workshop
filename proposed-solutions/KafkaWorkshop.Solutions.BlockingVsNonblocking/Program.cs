using Confluent.Kafka;

const string Topic = "kafka-exercise-producer-1";

var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092,localhost:9093,localhost:9094"
};

var producer = new ProducerBuilder<string, string>(producerConfig)
    .SetKeySerializer(Serializers.Utf8)
    .SetValueSerializer(Serializers.Utf8)
    .Build();

ProduceNonBlocking(Topic, producer);
//await ProduceBlocking(Topic, producer);

void ProduceNonBlocking(string topic, IProducer<string, string> producer)
{
    for (int i = 0; i < 5; i++)
    {
        var message = new Message<string, string>
        {
            Key = i.ToString(),
            Value = DateTime.Now.ToString()
        };
 
        producer.Produce(Topic, message, report =>
        {
            Console.WriteLine("Produced message to topic {0} partition {1} @ offset {2}",
                report.TopicPartition.Topic,
                report.TopicPartition.Partition,
                report.Offset);
        });
    }
}

async Task ProduceBlocking(string topic, IProducer<string, string> producer)
{
    for (int i = 0; i < 5; i++)
    {
        var message = new Message<string, string>
        {
            Key = i.ToString(),
            Value = DateTime.Now.ToString()
        };

        var report = await producer.ProduceAsync(Topic, message);
        
        Console.WriteLine("Produced message to topic {0} partition {1} @ offset {2}",
            report.TopicPartition.Topic,
            report.TopicPartition.Partition,
            report.Offset);
    }
}