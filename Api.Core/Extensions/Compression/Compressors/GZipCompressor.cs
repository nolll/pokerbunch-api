using System.IO;
using System.IO.Compression;

namespace Api.Extensions.Compression.Compressors
{
    public class GZipCompressor : Compressor
    {
        private const string GZipEncoding = "gzip";

        public override string EncodingType => GZipEncoding;

        protected override Stream CreateCompressionStream(Stream output)
        {
            return new GZipStream(output, CompressionMode.Compress, leaveOpen: true);
        }

        protected override Stream CreateDecompressionStream(Stream input)
        {
            return new GZipStream(input, CompressionMode.Decompress, leaveOpen: true);
        }
    }
}