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

        // преобразуем параметры, которые мы получили в запросе, в словарь
        var paramsDict = input.Params.Deserialize<Dictionary<string, JsonElement>>()!;

        // создаём массив для значений параметров
        var parametersArray = new object?[parameters.Count];

        foreach (var p in paramsDict)
        {
            // для каждого параметра, который ожидает метод, получаем значение и конвертируем его
            // строковое представление с помощью класса Convert
            if (parameters.ContainsKey(p.Key))
            {
                var type = parameters[p.Key];
                parametersArray[Array.IndexOf(parameterKeys, p.Key)] = Convert.ChangeType(p.Value.ToString(), type);
            }
        }

        // вызываем метод с подготовленными параметрами
        return await InvokeMethod(method, rpcService, parametersArray);
    }

    private async Task<RpcResult> InvokeMethod(MethodInfo method, IRpcService service, Object?[] parametersArray)
    {
        if (method.ReturnType == typeof(Task))
        {
            var task = (Task)method.Invoke(service, parametersArray)!;

            await task;
            return new RpcResult
            {
                Method = method.Name,
                Status = 200
            };
        }
        else if (method.ReturnType.IsGenericType && method.ReturnType.BaseType == typeof(Task))
        {
            var task = (Task)method.Invoke(service, parametersArray)!;

            await task;
            return new RpcResult
            {
                Method = method.Name,
                Result = task.GetType().GetProperty("Result")!.GetValue(task),
                Status = 200
            };
        }
        else if (method.ReturnType == typeof(void))
        {
            method.Invoke(service, parametersArray);
            return new RpcResult
            {
                Method = method.Name,
                Status = 200
            };
        }
        else
        {
            var result = method.Invoke(service, parametersArray);
            return new RpcResult
            {
                Method = method.Name,
                Result = result,
                Status = 200
            };
        }
    }
}