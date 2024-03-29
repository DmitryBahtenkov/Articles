﻿using System.Text.Json;
using System.Threading.Channels;
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
    
    public async Task<TData> GetOrAdd<TData>(string key, Func<Task<TData>> callback)
    {
        // пытаемся получить данные из кэша
        var existingData = await _database.StringGetAsync(key);
        // если данные существуют, возвращаем их в виде необходимого объекта
        if (existingData.HasValue)
        {
            return JsonSerializer.Deserialize<TData>(existingData.ToString())!;
        }

        // получаем данные из колбэк-функции
        var data = await callback();
        // сохраняем данные с помощью метода, написанного ранее, с истечением через минуту
        await _database.StringSetAsync(key, JsonSerializer.Serialize(data), expiry: TimeSpan.FromMinutes(1));
        
        return data;
    }

    public async Task Publish<TEvent>(TEvent @event)
    {
        // выполняем метод Publish для публикации сообщения в Redis
        // первым параметром методя является название канала, в который мы публикуем сообщение
        // в данном случае это название типа события, например "CreateUserEvent"
        await _database.PublishAsync(typeof(TEvent).Name, JsonSerializer.Serialize(@event));
    }

    public async Task Subscribe<TEvent>(Action<TEvent> callback)
    {
        // Из объекта ConnectionMultiplexer Redis позволяяет получить объект ISubscriber
        // с помощью которого можно подписаться на события в определённом канале
        await _database.Multiplexer
            .GetSubscriber()
            .SubscribeAsync(
                typeof(TEvent).Name,
                        // выполняем callback-функцию с десериализованным сообщением в параметрах
                (channel, value) => callback(JsonSerializer.Deserialize<TEvent>(value!)!));
    }
}