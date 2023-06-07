namespace CustomCache;

public interface ICustomMemoryCache
{
    /// <summary>
    /// Получить элемент из кэша или добавить новый
    /// </summary>
    /// <param name="key">Ключ для доступа к данным</param>
    /// <param name="ttl">Время жизни данных при сохранении</param>
    /// <param name="getter">Функция для получения данных</param>
    /// <typeparam name="T">Тип объекта, который пытаемся получить</typeparam>
    /// <returns></returns>
    public Task<T> GetOrAdd<T>(string key, int ttl, Func<Task<T>> getter);
    /// <summary>
    /// Инвалидировать кэш
    /// </summary>
    /// <param name="keys">список ключей, по которым необходимо очистить данные. Если пустой, очищаем весь кэш</param>
    public void Invalidate(params string[] keys);
    /// <summary>
    /// Проверить, существует ли такой ключ в кэше
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey(string key);
}