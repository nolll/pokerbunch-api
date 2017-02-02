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
            var result = Sut.Execute(Request);
            Assert.AreEqual(DisplayName, result.Name);
        }

        [Test]
        public void DescriptionIsSet()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual(Description, result.Description);
        }

        [Test]
        public void HouseRulesIsSet()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual(HouseRules, result.HouseRules);
        }

        [Test]
        public void CanEditIsFalse()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual(Role.Player, result.Role);
        }
    }
}
