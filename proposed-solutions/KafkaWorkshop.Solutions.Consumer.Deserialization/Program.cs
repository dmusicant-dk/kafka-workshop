// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using KafkaWorkshop.Solutions.Shared.Contracts;
using KafkaWorkshop.Solutions.Shared.Serde;

const string topic = "kafka-exercise-consumer-1";

// main token, so we cancel
var cts = new CancellationTokenSource(); // so we can stop

// capture ctrl+c 
Console.CancelKeyPress += (_, _) => {
	cts.Cancel();
}; 
	
	
// prepare config
var consumerConfig = new ConsumerConfig {
	BootstrapServers = "localhost:9092",
	GroupId = Guid.NewGuid().ToString(), // this will allow us to test fro the beginning everytime
	AutoOffsetReset =
		AutoOffsetReset
			.Earliest, // when first time in the group - begin from start, combined with the above, will let us rerun 
};

// prepare consumer
var consumer = new ConsumerBuilder<long, User?>(consumerConfig)
	.SetKeyDeserializer(Deserializers.Int64) // default long deserializers
	.SetValueDeserializer(new UserDeserializer()) // custom json deserializer for User Record 
	.Build();

consumer.Subscribe(topic);

// main consumer loop
while (!cts.IsCancellationRequested) {
	// consumer result has various data, including topic/partition, and the actual message is inside message 
	ConsumeResult<long, User?> incomingMessage = consumer.Consume(); // message wil be never null!


	if (incomingMessage.Message.Value is null) {
		Console.WriteLine($"Consumed record with key: {incomingMessage.Message.Key} and NULL value");
		continue;
	}
	
	Console.WriteLine($"Consumed record with key: {incomingMessage.Message.Key} and firstName: {incomingMessage.Message.Value.FirstName}");
}