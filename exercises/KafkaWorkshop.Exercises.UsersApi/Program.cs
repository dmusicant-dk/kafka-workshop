using KafkaWorkshop.Exercises.UsersApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<UsersConsumerService>();
builder.Services.AddSingleton<UsersDb>();
builder.Services.AddSingleton<UsersDbLoaderMonitor>();
builder.Services.AddSingleton<UsersProducer>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/users", (UsersDb db, UsersDbLoaderMonitor monitor) =>
{
});

app.MapGet("/users/{id:long}", (long id, UsersDb db, UsersDbLoaderMonitor monitor) =>
{
});

// Implement any other endpoints you need here in a similar fashion

app.Run();