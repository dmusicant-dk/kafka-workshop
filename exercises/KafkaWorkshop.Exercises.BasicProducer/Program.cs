using Confluent.Kafka;

namespace KafkaWorkshop.Exercises.BasicProducer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using var producer = new ProducerBuilder<int, string>(config)
                .Build();

            int count = 0;
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            while (await timer.WaitForNextTickAsync())
            {
                try
                {
                    var result = await producer.ProduceAsync("test-topic", new Message<int, string> { Key = count, Value = $"Message {count}"});
                    var toppar = result.TopicPartitionOffset;
                    Console.WriteLine($"Produced message '{result.Key}:{result.Value}' to offset: {toppar.Offset} of partition {toppar.Topic}:{toppar.Partition.Value}.");
                    count++;    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Producing failed: {e.Message}");
                }
            }
        }
    }
}