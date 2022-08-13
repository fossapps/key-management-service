using System.Reflection;
using API.Configs;
using Business.Exceptions;
using HotChocolate.Resolvers;
using HotChocolate.Types.Descriptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Attributes;

public class ProtectedAttribute : ObjectFieldDescriptorAttribute
{
    private readonly string _operation;

    public ProtectedAttribute(string operation)
    {
        _operation = operation;
    }

    public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Use(next => ctx =>
        {
            var operationRule = GetSecurityRequirements(ctx);
            if (operationRule == null)
            {
                return next.Invoke(ctx);
            }
            var headerValue = GetHeaderValue(ctx, operationRule);
            if (headerValue == null || headerValue != operationRule.Value)
            {
                throw new OperationNotPermitted($"you do not have access to perform this operation. ${headerValue}");
            }
            return next.Invoke(ctx);
        });
    }

    private string? GetHeaderValue(IMiddlewareContext ctx, SecurityRequirements rule)
    {
        ctx.ContextData.TryGetValue("HttpContext", out var ctxData);
        if (ctxData == null || ctxData.GetType() != typeof(DefaultHttpContext))
        {
            throw new ApplicationException("http context not configured correctly");
        }

        var httpContext = (DefaultHttpContext) ctxData;
        if (!httpContext.Request.Headers.TryGetValue(rule.Header, out var headerValue))
        {
            return null;
        }

        return headerValue;
    }
    private SecurityRequirements? GetSecurityRequirements(IMiddlewareContext ctx)
    {
        var securityOptions = ctx.Service<IOptions<Security>>().Value;
        if (securityOptions == null || securityOptions.Rules == null)
        {
            return null;
        }
        securityOptions.Rules.TryGetValue(_operation, out var securityRequirements);
        return securityRequirements;
    }
}
