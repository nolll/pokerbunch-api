using Core.Entities;

namespace Tests.Core.UseCases.GetBunchTests;

public class WithManager : Arrange
{
    protected override Role Role => Role.Manager;

    [Test]
    public void RoleIsManager() => Assert.AreEqual(Role.Manager, Result.Data.Role);
}