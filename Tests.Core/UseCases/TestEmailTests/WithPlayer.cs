using Core.Entities;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.TestEmailTests;

public class WithPlayer : Arrange
{
    protected override bool ExecuteAutomatically => false;
    protected override Role Role => Role.Manager;

    [Test]
    public void ThrowsException()
    {
        Assert.Throws<AccessDeniedException>(Execute);
    }
}