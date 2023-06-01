using Confluent.Kafka;

namespace KafkaWorkshop.Exercises.BasicConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            string consumerGroup = "test-consumer-group";
            if (args.Length == 1)
            {
                consumerGroup = args[0];
            }
            
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092", 
                GroupId = consumerGroup, 
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            using var consumer = new ConsumerBuilder<int, string>(config).Build();
            consumer.Subscribe("test-topic");

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cts.Token);
                        var message = consumeResult.Message;
                        var toppar = consumeResult.TopicPartitionOffset;
                        Console.WriteLine($@"Consumed message '{message.Key}:{message.Value}' at offset: {toppar.Offset} of partition {toppar.Topic}:{toppar.Partition.Value}.");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine("Error occured: {0}", e.Error.Reason);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}
