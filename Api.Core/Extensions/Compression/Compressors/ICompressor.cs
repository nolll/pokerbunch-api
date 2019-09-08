using System.IO;
using System.Threading.Tasks;

namespace Api.Extensions.Compression.Compressors
{
    public interface ICompressor
    {
        string EncodingType { get; }
        Task Compress(Stream source, Stream destination);
        Task Decompress(Stream source, Stream destination);
    }
}