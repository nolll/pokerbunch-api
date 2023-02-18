using Core.Entities;
using Core.Errors;

namespace Tests.Core.UseCases.ClearCacheTests;

public class WithPlayer : Arrange
{
    protected override Role Role => Role.Player;

    [Test]
    public void ReturnsError()
    {
        Assert.That(Result?.Success, Is.False);
        Assert.That(Result?.Error?.Type, Is.EqualTo(ErrorType.AccessDenied));
    }
}