using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Api.Extensions.Compression.Compressors;

namespace Api.Extensions.Compression;

public class CompressionHandler : DelegatingHandler
{
    private Collection<ICompressor> Compressors { get; }

    public CompressionHandler()
    {
        Compressors = new Collection<ICompressor>
        {
            new GZipCompressor(),
            new DeflateCompressor()
        };
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (request.Headers.AcceptEncoding.Any())
        {
            var encoding = request.Headers.AcceptEncoding.First();

            var compressor = Compressors.FirstOrDefault(c => c.EncodingType.Equals(encoding.Value, StringComparison.InvariantCultureIgnoreCase));

            if (compressor != null)
            {
                response.Content = new CompressedContent(response.Content, compressor);
            }
        }

        return response;
    }
}