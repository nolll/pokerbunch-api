using Core.Services;

namespace Tests.Core.Services;

public class EncryptionTests
{
    [Test]
    public void Encrypt()
    {
        const string expected = "425af12a0743502b322e93a015bcf868e324d56a";

        var result = EncryptionService.Encrypt("abcd", "efgh");
        Assert.That(result, Is.EqualTo(expected));
    }
}