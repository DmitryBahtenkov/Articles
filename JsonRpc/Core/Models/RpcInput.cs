using System.Text.Json;

namespace JsonRpc.Core.Models;

public class RpcInput
{
    public string Method { get; set; }
    public JsonDocument Params { get; set; }
}

public class RpcResult
{
    public string Method { get; set; }
    public object? Result { get; set; }
    public int Status { get; set; }
}