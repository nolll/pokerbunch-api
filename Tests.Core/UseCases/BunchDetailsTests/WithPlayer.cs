using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.BunchDetailsTests
{
    public class WithPlayer : Arrange
    {
        protected override Role Role => Role.Player;

        [Test]
        public void BunchNameIsSet()
        {
            Assert.AreEqual(DisplayName, Execute().BunchName);
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
            Assert.IsFalse(Execute().CanEdit);
        }
    }
}
