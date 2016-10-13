using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.GetBunchTests
{
    public class WithPlayer : Arrange
    {
        protected override Role Role => Role.Player;

        [Test]
        public void BunchNameIsSet()
        {
            Assert.AreEqual(DisplayName, Execute().Name);
        }

        [Test]
        public void DescriptionIsSet()
        {
            Assert.AreEqual(Description, Execute().Description);
        }

        [Test]
        public void HouseRulesIsSet()
        {
            Assert.AreEqual(HouseRules, Execute().HouseRules);
        }

        [Test]
        public void CanEditIsFalse()
        {
            Assert.AreEqual(Role.Player, Execute().Role);
        }
    }
}
