using Core.Entities;

namespace Tests.Core.UseCases.UserDetailsTests;

public class OtherUserAsAdmin : Arrange
{
    protected override Role Role => Role.Admin;

    [Test]
    public void CanViewAllIsTrue() => Assert.That(Result?.Data?.CanViewAll, Is.True);
}