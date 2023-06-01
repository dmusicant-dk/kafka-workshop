using KafkaWorkshop.Solutions.UsersApi;
using KafkaWorkshop.Solutions.Shared.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<UsersConsumerService>();
builder.Services.AddSingleton<UsersDb>();
builder.Services.AddSingleton<UsersDbLoaderMonitor>();
builder.Services.AddSingleton<UsersProducer>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

IResult dbNotReadyResult = Results.Problem("UsersDb is not loaded yet", statusCode: 503);

app.MapGet("/users", (UsersDb db, UsersDbLoaderMonitor monitor) =>
    monitor.Loaded ? 
        Results.Ok(db.GetAll()) : 
        dbNotReadyResult);

app.MapGet("/users/{id:long}", (long id, UsersDb db, UsersDbLoaderMonitor monitor) => monitor.Loaded switch
{
    true when db.GetUser(id) is not null => Results.Ok(db.GetUser(id)),
    true => Results.NotFound(),
    _ => dbNotReadyResult
});

app.MapPost("/users", async (User user, UsersProducer userProducer) =>
{
    await userProducer.ProduceUpdate(user);
    return Results.Created($"/users/{user.Id}", user);
});

app.MapPut("/users/{id:long}", async (long id, User user, UsersProducer userProducer) =>
{
    var tmpUser = user with { Id = id };
    await userProducer.ProduceUpdate(tmpUser);
    return Results.Ok(tmpUser);
});

app.MapDelete("/users/{id:long}", async (long id, UsersProducer userProducer) =>
{
    await userProducer.ProduceDelete(id);
    return Results.Ok();
});

app.MapGet("/healthz", (UsersDbLoaderMonitor monitor) => 
    monitor.Loaded ? 
        Results.Ok() : 
        dbNotReadyResult);

app.Run();