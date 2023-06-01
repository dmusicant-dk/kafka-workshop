using Confluent.Kafka;
using KafkaWorkshop.Exercises.Shared.Contracts;

const string Topic = "kafka-exercise-producer-2";

var users = new List<User>
{
    new(1, "John", "Doe", new DateTimeOffset(1980, 1, 1, 0, 0, 0, TimeSpan.Zero)),
    new(2, "Jane", "Doe", new DateTimeOffset(1981, 1, 1, 0, 0, 0, TimeSpan.Zero)),
    new(3, "John", "Smith", new DateTimeOffset(1982, 1, 1, 0, 0, 0, TimeSpan.Zero)),
    new(4, "Jane", "Smith", new DateTimeOffset(1983, 1, 1, 0, 0, 0, TimeSpan.Zero)),
};

// Setup your producer here

foreach (var user in users)
{
    // Produce your message here
}

Console.ReadKey(true);