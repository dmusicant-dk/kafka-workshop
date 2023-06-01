using Confluent.Kafka;

const string Topic = "kafka-exercise-producer-1";

// Setup your producer here
IProducer<string, string> producer = null!;

ProduceNonBlocking(Topic, producer);
//await ProduceBlocking(Topic, producer);

void ProduceNonBlocking(string topic, IProducer<string, string> producer)
{
    for (int i = 0; i < 5; i++)
    {
        // Put your non-blocking producer code here
    }
}

async Task ProduceBlocking(string topic, IProducer<string, string> producer)
{
    for (int i = 0; i < 5; i++)
    {
        // Put your blocking producer code here
    }
}