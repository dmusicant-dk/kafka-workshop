using Confluent.Kafka;
using KafkaWorkshop.Exercises.Consumer.Deserialization;

const string Topic = "kafka-exercise-consumer-1";

// main token, so we cancel
var cts = new CancellationTokenSource(); // so we can stop

// capture ctrl+c 
Console.CancelKeyPress += (_, _) => {
    cts.Cancel();
}; 

while (!cts.IsCancellationRequested) {
    // Put your consumer code here
}