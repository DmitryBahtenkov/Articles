using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RedisCases.Models;

namespace RedisCases.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly RedisService _redisService;

    public UserController(RedisService redisService)
    {
        _redisService = redisService;
    }

    [HttpPost]
    public async Task<User> CreateUser([FromBody] User user)
    {
        await _redisService.SetValue(user.Id.ToString(), user);
        
        return user;
    }

    [HttpGet("{id}")]
    public async Task<User?> GetUser(Guid id)
    {
        return await _redisService.GetValue<User>(id.ToString());
    }

    [HttpGet]
    public async Task<User[]> GetUsers()
    {
        // запускаем отсчёт времени выполнения кода
        var stopwatch = Stopwatch.StartNew();
        // получаем пользователей. В качестве параметра GetOrAdd мы передаём функцию
        // GetUsersFromFile, результат которой будет кеширован
        var result = await _redisService.GetOrAdd("users.json", GetUsersFromFile);
        // сохраняем время выполнения коде
        var time = stopwatch.ElapsedMilliseconds;
        Console.WriteLine("Result time: " + time);
        // останавливаем таймер
        stopwatch.Stop();
        return result;
    }

    private async Task<User[]> GetUsersFromFile()
    {
        // читаем файл users.json и сериализуем его в объект User[]
        using (var reader = new StreamReader("users.json"))
        {
            return JsonSerializer.Deserialize<User[]>(await reader.ReadToEndAsync())!;
        }
    }
}