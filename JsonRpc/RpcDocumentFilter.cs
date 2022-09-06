using JsonRpc.Core.Abstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JsonRpc;

public class RpcDocumentFilter : IDocumentFilter
{
    private readonly IRpcServiceHolder _rpcServiceHolder;

    public RpcDocumentFilter(IRpcServiceHolder rpcServiceHolder)
    {
        _rpcServiceHolder = rpcServiceHolder;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Paths.Clear();

        foreach (var (name, service) in _rpcServiceHolder.GetServices())
        {

        }
    }
}