using KafkaWorkshop.Exercises.Consumer.BasicStructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<UsersConsumerService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();