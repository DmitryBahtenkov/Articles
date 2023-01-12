using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nest;
using PgElasticSync.Elastic;
using PgElasticSync.Models;
using PgElasticSync.Pg;
using PgElasticSync.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PgContext>();
builder.Services.AddScoped<ElasticRepository>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
builder.Services.AddSingleton<ElasticClient>(s => new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200/"))));
builder.Services.AddHostedService<SubscribeBackgroundService>();
builder.Services.AddSingleton<RedisService>();

var app = builder.Build();

// специальная настройка для работы с датой в ПГ
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.MapGet("api/employees", async ([FromServices] EmployeeService service) => await service.All());

app.MapGet("api/employees/{id}", async (int id, [FromServices] EmployeeService service) => await service.ById(id));

app.MapPost("api/employees", async ([FromBody] Employee employee, [FromServices] EmployeeService service) => await service.Create(employee));

app.MapPut("api/employees", async ([FromBody] Employee employee, [FromServices] EmployeeService service) => await service.Update(employee));

app.MapDelete("api/employees/{id}", async (int id, [FromServices] EmployeeService service) => await service.Delete(id));

app.MapGet("api/employees/search", async (
    [FromServices] EmployeeService service,
    [FromServices] ILogger<EmployeeService> logger,
    string text, 
    string? city,
    string? university,
    DateTime? fromStartDate) => 
    {
        var result = await service.Search(text, city, university, fromStartDate, true);
        return result;
    });

app.Run();