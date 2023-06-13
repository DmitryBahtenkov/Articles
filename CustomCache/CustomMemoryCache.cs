using System.Collections.Concurrent;

namespace CustomCache;

public class CustomMemoryCache : ICustomMemoryCache
{
    // словарь для хранения данных в кэше
    private readonly ConcurrentDictionary<string, CacheItem> _cache;
    // связный список элементов кэша для отображения элементов кэша
    // в порядке их использования
    private readonly LinkedList<CacheItem> _cacheItems;
    // максимальный размер кэша
    private readonly int _capacity;

    public CustomMemoryCache(int capacity)
    {
        _capacity = capacity;
        _cache = new ConcurrentDictionary<string, CacheItem>();
        _cacheItems = new LinkedList<CacheItem>();
    }

    public async Task<T> GetOrAdd<T>(string key, int ttl, Func<Task<T>> getter)
    {
        // если элемент в кэше есть
        if (_cache.TryGetValue(key, out var item))
        {
            // если время истекло
            if (DateTime.UtcNow - item.LastAccess > TimeSpan.FromSeconds(item.TTL))
            {
                var data = await getter();
                // обновляем данные и TTL
                item.Value = data;
                item.TTL = ttl;
            }

            // проставляем текущую дату использования данных
            item.LastAccess = DateTime.UtcNow;
            // перемещаем элемент в начало списка, так как у него
            // самое свежее время использования
            MoveToHead(item);
            // При необходимости очищаем кэш от старых элементов
            // по политике LRU
            ClearLastRecentlyUsed();
            
            return item.GetValue<T>();
        }
        else
        {
            var data = await getter();
            var newItem = new CacheItem(key, data, ttl);
            // добавляем новую запись в кэш
            _cache[key] = newItem;
            // помещаем новый элемент в начало кэша
            newItem.Node = _cacheItems.AddFirst(newItem);
            // при необходимости очищаем кэш от старых данных
            ClearLastRecentlyUsed();
            
            return data;
        }
    }

    public void Invalidate(params string[] keys)
    {
        // если не указали ни одного ключа - очищаем весь кэш
        var keysForDelete = keys.Any() ? keys : _cache.Keys;
        foreach (var key in keysForDelete)
        {
            Delete(key);
        }
    }

    public bool ContainsKey(string key)
    {
        return _cache.ContainsKey(key);
    }

    private void ClearLastRecentlyUsed()
    {
        while (_cache.Count > _capacity)
        {
            // Удаляем последний элемент из связного списка и словаря
            var oldestItem = _cacheItems.Last?.Value;

            if (oldestItem is not null)
            {
                Delete(oldestItem.Key);
            }
        }
    }
    
    private void Delete(string key)
    {
        if (_cache.TryRemove(key, out var item))
        {
            // Удаляем элемент из связного списка
            if (item.Node is not null)
            {
                _cacheItems.Remove(item.Node);
            }
        }
    }
    
    // Метод для перемещения элемента в начало связного списка
    private void MoveToHead(CacheItem item)
    {
        // Если элемент уже находится в начале списка, то ничего не делаем
        if (item.Node == _cacheItems.First) return;

        // Если элемент находится в середине или в конце списка, то удаляем его из списка
        if (item.Node != null)
        {
            _cacheItems.Remove(item.Node);
        }

        // Добавляем элемент в начало списка и сохраняем ссылку на узел
        item.Node = _cacheItems.AddFirst(item);
    }
}