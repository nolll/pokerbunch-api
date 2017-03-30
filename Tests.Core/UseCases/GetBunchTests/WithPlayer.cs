using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.GetBunchTests
{
    public class WithPlayer : Arrange
    {
        protected override Role Role => Role.Player;

        [Test]
        public void BunchNameIsSet() => Assert.AreEqual(DisplayName, Result.Name);

        [Test]
        public void DescriptionIsSet() => Assert.AreEqual(Description, Result.Description);

        [Test]
        public void HouseRulesIsSet() => Assert.AreEqual(HouseRules, Result.HouseRules);

        [Test]
        public void CanEditIsFalse() => Assert.AreEqual(Role.Player, Result.Player.Role);
    }
}
