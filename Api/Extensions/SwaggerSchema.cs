using System;

namespace Api.Extensions;

public class SwaggerSchema
{
    public static string GetSwaggerTypeName(Type type)
    {
        return type.Name;
    }
}