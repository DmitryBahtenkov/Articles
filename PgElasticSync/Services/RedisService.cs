using System.Text.Json;
using PgElasticSync.Models;
using StackExchange.Redis;

namespace PgElasticSync.Services
{
    public class RedisService
    {
        private readonly IConnectionMultiplexer _connection;

        public RedisService(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        public async Task Publish<TEvent>(string channel, TEvent data)
        {
            await _connection.GetDatabase().PublishAsync(channel, JsonSerializer.Serialize(data)); 
        }

        public async Task Subscribe<TEvent>(string channel, Func<TEvent, Task> callback)
        {
            await _connection.GetSubscriber().SubscribeAsync(channel, async(_, data) => 
            {
                var deserialized = JsonSerializer.Deserialize<TEvent>(data);
                if (deserialized is not null)
                {
                    try
                    {
                        await callback(deserialized);
                    }
                    catch(Exception)
                    {
                        // error handling
                    }
                }
            });
        }
    }

    record IndexEmployeeEvent(Employee Employee);
}