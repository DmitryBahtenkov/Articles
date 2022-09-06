using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCases.Controllers;

[Route("api/rate-limit")]
[ApiController]
public class RateLimitController : ControllerBase
{
    private readonly RedisService _redisService;

    public RateLimitController(RedisService redisService)
    {
        _redisService = redisService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // создаём ключ для хранения в Redis
        var key = $"{Request.Path.Value}:{DateTime.UtcNow:hh:mm}";
        // выполняем скрипт. В параметрах указываем время жизни записи (60 секунд) и максимальное кол-во запросов
        var res = await _redisService.ExecuteScript(Scripts.RateLimitScript, new
        {
            key = new RedisKey(key),
            expiry = 60,
            maxRequests = 10
        });

        return res == 0 ? Ok() : StatusCode(429);
    }
}