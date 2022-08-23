using JsonRpc.Core.Abstractions;
using JsonRpc.Core.Implementations;
using JsonRpc.Core.Models;
using JsonRpc.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// добавляем наши сервисы в DI-контейнер
builder.Services
    .AddScoped<IRpcServiceHolder, RpcServiceHolder>()
    .AddScoped<IRpcExecutor, RpcExecutor>()
    .AddScoped<TestRpcService>()
    .AddScoped<IRpcService, TestRpcService>(x => x.GetRequiredService<TestRpcService>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// с помощью механизма minimal api создаём эндпоинт для вызова rpc
app.MapPost("api/{service}/execute", async (
    string service,
    [FromServices] IRpcExecutor rpcExecutor,
    [FromBody] RpcInput input) => await rpcExecutor.ExecuteMethod(service, input));

app.Run();