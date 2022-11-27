using Core.Entities;

namespace Tests.Core.UseCases.GetBunchTests;

public class WithManager : Arrange
{
    protected override Role Role => Role.Manager;

    [Test]
    public void RoleIsManager() => Assert.That(Result.Data.Role, Is.EqualTo(Role.Manager));
}