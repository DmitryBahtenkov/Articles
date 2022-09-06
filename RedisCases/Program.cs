using RedisCases;
using RedisCases.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
builder.Services.AddSingleton<RedisService>();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

var redisService = app.Services.GetRequiredService<RedisService>();
await redisService.Subscribe<CreateUserEvent>((@event =>
{
    // выполнение какой-то полезной логики, например отправка email
    // новому пользователю
    Console.WriteLine($"User with id {@event.UserId} created");
}));

app.Run();