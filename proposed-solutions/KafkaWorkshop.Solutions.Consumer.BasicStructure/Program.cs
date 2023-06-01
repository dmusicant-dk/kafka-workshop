using KafkaWorkshop.Solutions.Consumer.BasicStructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddHostedService<UsersConsumerService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();