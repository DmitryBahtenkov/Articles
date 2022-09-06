namespace JsonRpc.Core.Abstractions;

public interface IRpcServiceHolder
{
    public IRpcService GetService(string input);
    public Dictionary<string, IRpcService> GetServices();
}