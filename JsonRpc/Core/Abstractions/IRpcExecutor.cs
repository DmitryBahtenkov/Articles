using JsonRpc.Core.Models;

namespace JsonRpc.Core.Abstractions;

public interface IRpcExecutor
{
    public Task<RpcResult> ExecuteMethod(string service, RpcInput input);
}