using Confluent.Kafka;

public class RetentionPolicyTestDataPublisher : TestDataPublisher
{
    private readonly IProducer<string, string> _producer;
    private readonly Random _random = new();
    private string _topic;

    public RetentionPolicyTestDataPublisher(string topic)
    {
        _topic = topic;
        
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092,localhost:9093,localhost:9094"
        };

        _producer = new ProducerBuilder<string, string>(producerConfig)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();
    }
    
    public override async Task Run(CancellationToken token)
    {
        for (int i = 0; i < 50; i++)
        {
            var msg = new Message<string, string>
            {
                Key = _random.Next(1, 10).ToString(),
                Value = DateTime.Now.ToString()
            };
            
            await _producer.ProduceAsync(_topic, msg, token);
            Console.WriteLine($"Produced with Key: {msg.Key} and Value:{msg.Value} to {_topic}");
        }
        
        while (!token.IsCancellationRequested)
        {
            var msg = new Message<string, string>
            {
                Key = _random.Next(1, 10).ToString(),
                Value = DateTime.Now.ToString()
            };
            
            await _producer.ProduceAsync(_topic, msg, token);
            Console.WriteLine($"Produced with Key: {msg.Key} and Value:{msg.Value} to {_topic}");

            await Task.Delay(500, token);
        }
    }
}