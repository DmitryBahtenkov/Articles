using System.Reflection;
using JsonRpc.Core.Abstractions;

namespace JsonRpc.Core.Implementations;

public class RpcServiceHolder : IRpcServiceHolder
{
    private readonly Dictionary<string, IRpcService> _rpcServicesDict;

    public RpcServiceHolder(IEnumerable<IRpcService> rpcServices)
    {
        _rpcServicesDict = rpcServices.Select(x =>
        {
            var type = x.GetType();

            var attr = type.GetCustomAttribute<RpcAttribute>();

            var key = attr is not null ? attr.Name : type.Name;

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
}