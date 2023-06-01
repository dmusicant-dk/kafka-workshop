static ITestDataPublisher CreatePublisher(string excercise)
{
    return excercise switch
    {
        "deserialization" => new UserDataPublisher("kafka-exercise-consumer-1"),
        "users-api-data" => new UserDataPublisher("kafka-exercise-user-data-topic"),
        "retention-policy-delete-publisher" => new RetentionPolicyTestDataPublisher("expiring-topic"),
        "retention-policy-compact-publisher" => new RetentionPolicyTestDataPublisher("compacted-topic"),
        "retention-policy-compact-delete-publisher" => new RetentionPolicyTestDataPublisher("compacted-expiring-topic"),
        _ => throw new ArgumentException()
    };
}

string exercise = args[0];

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cts.Cancel();
};

var publisher = CreatePublisher(exercise);

_ = Task.Run(() =>
{
    var running = true;
    while (!cts.IsCancellationRequested)
    {
        var key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.Spacebar) continue;

        if (running)
        {
            publisher.Stop();
            running = false;
            Console.WriteLine("Publisher stopped.");
        }
        else
        {
            publisher.Start();
            running = true;
            Console.WriteLine("Publisher started.");
        }
    }
});

await publisher.Run(cts.Token);