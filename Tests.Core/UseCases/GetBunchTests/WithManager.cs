using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.GetBunchTests
{
    public class WithManager : Arrange
    {
        protected override Role Role => Role.Manager;

        [Test]
        public void RoleIsManager() => Assert.AreEqual(Role.Manager, Result.Player.Role);
    }
}