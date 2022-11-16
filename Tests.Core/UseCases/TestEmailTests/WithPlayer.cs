using Core.Entities;
using Core.Errors;
using NUnit.Framework;

namespace Tests.Core.UseCases.TestEmailTests;

public class WithPlayer : Arrange
{
    protected override Role Role => Role.Manager;

    [Test]
    public void ReturnsError()
    {
        Assert.That(Result.Error.Type, Is.EqualTo(ErrorType.AccessDenied));
    }
}