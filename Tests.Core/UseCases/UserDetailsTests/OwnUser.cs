using NUnit.Framework;

namespace Tests.Core.UseCases.UserDetailsTests;

public class OwnUser : Arrange
{
    protected override bool ViewingOwnUser => true;

    [Test]
    public void CanViewAllIsTrue() => Assert.IsTrue(Result.CanViewAll);
}