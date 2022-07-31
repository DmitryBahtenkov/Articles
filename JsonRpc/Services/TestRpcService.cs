using JsonRpc.Core.Abstractions;

namespace JsonRpc.Services;

public class TestRpcService : IRpcService
{
    public Task<int> Add(int a, int b)
    {
        return Task.FromResult(a + b);
    }
}