using System.IO;
using System.IO.Compression;

namespace Api.Extensions.Compression.Compressors;

public class DeflateCompressor : Compressor
{
    private const string DeflateEncoding = "deflate";

    public override string EncodingType => DeflateEncoding;

    protected override Stream CreateCompressionStream(Stream output)
    {
        return new DeflateStream(output, CompressionMode.Compress, leaveOpen: true);
    }

    protected override Stream CreateDecompressionStream(Stream input)
    {
        return new DeflateStream(input, CompressionMode.Decompress, leaveOpen: true);
    }
}