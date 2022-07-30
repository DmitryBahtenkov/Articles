using System.Reflection;
using System.Text.Json;
using JsonRpc.Core.Abstractions;
using JsonRpc.Core.Models;

namespace JsonRpc.Core.Implementations;

public class RpcExecutor : IRpcExecutor
{
    private readonly IRpcServiceHolder _rpcServiceHolder;

    public RpcExecutor(IRpcServiceHolder rpcServiceHolder)
    {
        _rpcServiceHolder = rpcServiceHolder;
    }

    public async Task<RpcResult> ExecuteMethod(string service, RpcInput input)
    {
        var rpcService = _rpcServiceHolder.GetService(service);

        var method = rpcService.GetType().GetMethod(input.Method);

        if (method is null)
        {
            throw new MissingMethodException();
        }

        var parameters = method
            .GetParameters()
            .ToDictionary(x => x.Name!, x => x.ParameterType);

        var parameterKeys = parameters.Keys.ToArray();

        if (!parameters.Any())
        {
            return await InvokeMethod(method, rpcService, Array.Empty<Object>());
        }

        var paramsDict = input.Params.Deserialize<Dictionary<string, JsonElement>>()!;
        
        var parametersArray = new object?[parameters.Count];

        foreach (var p in paramsDict)
        {
            if (parameters.ContainsKey(p.Key))
            {
                var type = parameters[p.Key];
                parametersArray[Array.IndexOf(parameterKeys, p.Key)] = Convert.ChangeType(p.Value.ToString(), type);
            }
        }
        
        return await InvokeMethod(method, rpcService, parametersArray);
    }
    
    private async Task<RpcResult> InvokeMethod(MethodInfo method, IRpcService service, Object?[] parametersArray)
    {
        if (method.ReturnType == typeof(Task))
        {
            var task = (Task?)method.Invoke(service, parametersArray);
            if (task is not null)
            {
                await task;
                return new RpcResult
                {
                    Method = method.Name
                };
            }
        }
        else if (method.ReturnType.IsGenericType && method.ReturnType.BaseType == typeof(Task))
        {
            var task = (Task?) method.Invoke(service, parametersArray);
            if (task is not null)
            {
                await task.ConfigureAwait(false);
                return new RpcResult
                {
                    Method = method.Name,
                    Result = task.GetType().GetProperty("Result")!.GetValue(task)
                };
            }
        }
        else if (method.ReturnType == typeof(void))
        {
            method.Invoke(service, parametersArray);
            return new RpcResult
            {
                Method = method.Name
            };
        }
        else
        {
            var result = method.Invoke(service, parametersArray);
            return new RpcResult
            {
                Method = method.Name,
                Result = result
            };
        }

        return new RpcResult();
    }
}