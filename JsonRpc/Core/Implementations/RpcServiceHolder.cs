using System.Reflection;
using JsonRpc.Core.Abstractions;

namespace JsonRpc.Core.Implementations;

public class RpcServiceHolder : IRpcServiceHolder
{
    private readonly Dictionary<string, IRpcService> _rpcServicesDict;

    // получаем из DI-контейнера массив реализаций IRpcService
    public RpcServiceHolder(IEnumerable<IRpcService> rpcServices)
    {
        // формируем словарь `название сервиса -> сервис` для удобного
        // получения реализаций по названию
        _rpcServicesDict = rpcServices.Select(x =>
        {
            // получаем тип конкретного сервиса для использования рефлексии
            var type = x.GetType();

            // получаем атрибут RpcAttribute
            var attr = type.GetCustomAttribute<RpcAttribute>();

            // если на сервис назначен атрибут, берём название оттуда,
            // иначе просто название типа
            var key = attr is not null ? attr.Name : type.Name;

            // возвращаем две переменные для построения словаря - ключ и значение 
            return (Key: key.ToLower(), Value: x);
        }).ToDictionary(x => x.Key, x => x.Value);
    }

    public IRpcService GetService(string input)
    {
        var lower = input.ToLower();
        if (_rpcServicesDict.ContainsKey(lower))
        {
            return _rpcServicesDict[lower];
        }

        throw new Exception("Service not found");
    }

    public Dictionary<string, IRpcService> GetServices()
    {
        return _rpcServicesDict;
    }
}