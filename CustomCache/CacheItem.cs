using System.Runtime.InteropServices;

namespace CustomCache;

public class CacheItem
{
    /// <summary>
    /// Ключ элемента
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Значение элемента
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Время жизни элемента в секундах
    /// </summary>
    public int TTL { get; set; }

    /// <summary>
    /// Время последнего обращения к элементу
    /// </summary>
    public DateTime LastAccess { get; set; }
    /// <summary>
    /// Размер данных в байтах 
    /// </summary>
    public int Size => Marshal.SizeOf(Value);
    /// <summary>
    /// Узел связного списка,
    /// в котором находится элемент кэша
    /// </summary>
    public LinkedListNode<CacheItem>? Node { get; set; }
    public CacheItem(string key, object value, int ttl)
    {
        Key = key;
        Value = value;
        TTL = ttl;
        LastAccess = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Преобразовать значение в какой-то тип
    /// </summary>
    public T GetValue<T>()
    {
        return Value is T value ? value : throw new InvalidCastException();
    }
}