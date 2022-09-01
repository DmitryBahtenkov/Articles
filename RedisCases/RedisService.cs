using System.Text.Json;
using StackExchange.Redis;

namespace RedisCases;

public class RedisService
{
    // объект IDatabase, описывающий отдельную БД в Redis
    private readonly IDatabase _database;

    // в конструкторе указана зависимость IConnecctionMultiplexer
    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        //  из коннекта к редису получаем объект БД для взаимодействия
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task SetValue<T>(string key, T value)
    {
        // используем метод SetStringAsync для установки значения по ключу. 
        // Метод JsonSerializer.Serialize преобразует объект в json-строку
        await _database.StringSetAsync(key, JsonSerializer.Serialize(value));
    }

    public async Task<T?> GetValue<T>(string key)
    {
        // получаем строку из БД. Переменная value имеет тип RedisValue
        var value = await _database.StringGetAsync(key);
        if (value.HasValue)
        {
            // преобразуем  json-строку в объект с помощью JsonSerializer.Deserialize 
            return JsonSerializer.Deserialize<T>(value.ToString());
        }

        return default;
    }

    public async Task<int> ExecuteScript(LuaScript script, object? parameters = null)
    {
        return (int)await _database.ScriptEvaluateAsync(script, parameters);
    }
}