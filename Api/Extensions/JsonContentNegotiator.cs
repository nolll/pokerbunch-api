﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Api.Extensions;

public class JsonContentNegotiator : IContentNegotiator
{
    private readonly JsonMediaTypeFormatter _jsonFormatter;

    public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
    {
        _jsonFormatter = formatter;
    }

    public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
    {
        return new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
    }
}