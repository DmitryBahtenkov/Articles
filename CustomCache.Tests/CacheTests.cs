namespace CustomCache.Tests;

public class CacheTests
{
    [Fact]
    public async Task GetOrAdd_ShouldReturnDataFromGetter_WhenKeyIsNotInCache()
    {
        // Arrange
        var cache = new CustomMemoryCache(1000);
        var key = "test";
        var ttl = 10;
        var data = 42;

        // Act
        var result = await cache.GetOrAdd(key, ttl, () => Task.FromResult(data));

        // Assert
        Assert.Equal(data, result);
    }

    [Fact]
    public async Task GetOrAdd_ShouldReturnDataFromCache_WhenKeyIsInCacheAndNotExpired()
    {
        // Arrange
        var cache = new CustomMemoryCache(1000);
        var key = "test";
        var ttl = 10;
        var data = 42;
        await cache.GetOrAdd(key, ttl, () => Task.FromResult(data)); // добавляем данные в кэш

        // Act
        var result =
            await cache.GetOrAdd(key, ttl, () => Task.FromResult(data + 1)); // пытаемся получить данные из кэша

        // Assert
        Assert.Equal(data, result); // должны получить старые данные, а не новые
    }

    [Fact]
    public async Task GetOrAdd_ShouldReturnDataFromGetter_WhenKeyIsInCacheAndExpired()
    {
        // Arrange
        var cache = new CustomMemoryCache(1000);
        var key = "test";
        var ttl = 1; // устанавливаем очень маленькое время жизни данных
        var data = 42;
        await cache.GetOrAdd(key, ttl, () => Task.FromResult(data)); // добавляем данные в кэш

        // Act
        await Task.Delay(2000); // ждем больше, чем время жизни данных
        var result =
            await cache.GetOrAdd(key, ttl, () => Task.FromResult(data + 1)); // пытаемся получить данные из кэша

        // Assert
        Assert.Equal(data + 1, result); // должны получить новые данные, а не старые
    }

    [Fact]
    public async Task GetOrAdd_ShouldRemoveLeastRecentlyUsedItem_WhenCacheIsFull()
    {
        // Arrange
        var cache = new CustomMemoryCache(2);
        var key1 = "test1";
        var key2 = "test2";
        var key3 = "test3";
        var ttl = 10;
        var data1 = 42;
        var data2 = 43;
        var data3 = 44;

        await cache.GetOrAdd(key1, ttl, () => Task.FromResult(data1)); // добавляем первые данные в кэш
        await cache.GetOrAdd(key2, ttl, () => Task.FromResult(data2)); // добавляем вторые данные в кэш

        // Act
        await cache.GetOrAdd(key3, ttl, () => Task.FromResult(data3)); // добавляем третьи данные в кэш

        // Assert
        Assert.False(cache.ContainsKey(key1)); // первые данные должны быть удалены из кэша как наименее используемые
        Assert.True(cache.ContainsKey(key2)); // вторые данные должны остаться в кэше
        Assert.True(cache.ContainsKey(key3)); // третьи данные должны остаться в кэше
    }
}