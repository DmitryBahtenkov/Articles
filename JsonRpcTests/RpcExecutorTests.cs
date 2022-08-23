using System.Text.Json;
using JsonRpc.Core.Abstractions;
using JsonRpc.Core.Implementations;
using JsonRpc.Core.Models;

namespace JsonRpcTests;

public class RpcExecutorTests
{
    private readonly IRpcExecutor _rpcExecutor;
    private readonly TestRpcService _testRpcService;

    public RpcExecutorTests()
    {
        _testRpcService = new TestRpcService();
        var holder = new RpcServiceHolder(new[] { _testRpcService });
        _rpcExecutor = new RpcExecutor(holder);
    }

    private readonly string _paramsJson = JsonSerializer.Serialize(new Dictionary<string, object>
    {
        { "a", 1 },
        { "b", 2 }
    });

    [Fact]
    public async Task SyncAddTest()
    {
        var input = new RpcInput
        {
            Method = nameof(TestRpcService.Add),
            Params = JsonDocument.Parse(_paramsJson)
        };

        var result = await _rpcExecutor.ExecuteMethod(nameof(TestRpcService), input);
        
        Assert.Equal(3, result.Result);
    }

    [Fact]
    public async Task AsyncAddTest()
    {
        var input = new RpcInput
        {
            Method = nameof(TestRpcService.AddAsync),
            Params = JsonDocument.Parse(_paramsJson)
        };

        var result = await _rpcExecutor.ExecuteMethod(nameof(TestRpcService), input);
        
        Assert.Equal(3, result.Result);
    }

    [Fact]
    public async Task SyncAddInternalTest()
    {
        var input = new RpcInput
        {
            Method = nameof(TestRpcService.AddInternal),
            Params = JsonDocument.Parse(_paramsJson)
        };

        var _ = await _rpcExecutor.ExecuteMethod(nameof(TestRpcService), input);
        
        Assert.Equal(3, _testRpcService.Sum);
    }

    [Fact]
    public async Task AsyncAddInternalTest()
    {
        var input = new RpcInput
        {
            Method = nameof(TestRpcService.AddInternalAsync),
            Params = JsonDocument.Parse(_paramsJson)
        };

        var _ = await _rpcExecutor.ExecuteMethod(nameof(TestRpcService), input);
        
        Assert.Equal(3, _testRpcService.Sum);
    }
}

public class TestRpcService : IRpcService
{
    public int Sum { get; private set; }
    
    public int Add(int a, int b)
    {
        return a + b;
    }

    public Task<int> AddAsync(int a, int b)
    {
        return Task.FromResult(a + b);
    }
    public void AddInternal(int a, int b)
    {
        Sum = a + b;
    }
    
    public Task AddInternalAsync(int a, int b)
    {
        Sum = a + b;
        return Task.CompletedTask;
    }
}