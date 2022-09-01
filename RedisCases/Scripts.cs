using StackExchange.Redis;

namespace RedisCases;

public static class Scripts
{
    public static LuaScript RateLimitScript => LuaScript.Prepare(RateLimiter);

    private const string RateLimiter = @"
            local requests = redis.call('INCR',@key)
            redis.call('EXPIRE', @key, @expiry)
            if requests < tonumber(@maxRequests) then
                return 0
            else
                return 1
            end
            ";
}