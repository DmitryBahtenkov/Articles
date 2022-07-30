namespace JsonRpc.Core.Abstractions;

[AttributeUsage(AttributeTargets.Class)]
public class RpcAttribute : Attribute
{
    public RpcAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}