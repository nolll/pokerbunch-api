﻿using System.IO;
using System.Net;
using System.Net.Http;
using Api.Extensions.Compression.Compressors;

namespace Api.Extensions.Compression;

public class CompressedContent : HttpContent
{
    private readonly HttpContent _content;
    private readonly ICompressor _compressor;

    public CompressedContent(HttpContent content, ICompressor compressor)
    {
        _content = content ?? throw new ArgumentNullException(nameof(content));
        _compressor = compressor ?? throw new ArgumentNullException(nameof(compressor));

        AddHeaders();
    }

    protected override bool TryComputeLength(out long length)
    {
        length = -1;
        return false;
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        using (_content)
        {
            var contentStream = await _content.ReadAsStreamAsync();
            await _compressor.Compress(contentStream, stream);
        }
    }

    private void AddHeaders()
    {
        foreach (var header in _content.Headers)
        {
            Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        Headers.ContentEncoding.Add(_compressor.EncodingType);
    }
}