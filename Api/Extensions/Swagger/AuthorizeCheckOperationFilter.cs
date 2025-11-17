using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Api.Extensions.Swagger;

/*
public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!context.ApiDescription.TryGetMethodInfo(out var methodInfo))
            return;

        if (!RequiresAuth(methodInfo))
            return;

        if (operation.Responses.All(r => r.Key != StatusCodes.Status401Unauthorized.ToString()))
            operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = "Unauthorized" });
        
        if (operation.Responses.All(r => r.Key != StatusCodes.Status403Forbidden.ToString()))
            operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = "Forbidden" });

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>() // policy names goes here
                }
            }
        };
    }

    private static bool RequiresAuth(MemberInfo methodInfo)
    {
        if (methodInfo.MemberType != MemberTypes.Method) 
            return false;
        
        var controllerHasAttribute = methodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ?? false;
        var methodAllowsAnonymous = methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
        var methodRequiresAuth = methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        return controllerHasAttribute
            ? !methodAllowsAnonymous
            : methodRequiresAuth;
    }
}
*/