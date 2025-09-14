using Core.Services;

namespace Tests.Core.Services;

public class EncryptionTests
{
    [Test]
    public void Encrypt() => 
        EncryptionService.Encrypt("abcd", "efgh").Should().Be("425af12a0743502b322e93a015bcf868e324d56a");
}