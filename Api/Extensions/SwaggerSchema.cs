﻿using System;

namespace Api.Extensions;

public static class SwaggerSchema
{
    public static string GetSwaggerTypeName(Type type)
    {
        return type.Name;
    }
}