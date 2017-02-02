using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.CurrentCashgamesTests
{
    public class WithoutGames : Arrange
    {
        protected override Role Role => Role.Player;

        [Test]
        public void ReturnsEmptyList()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual(0, result.Games.Count);
        }
    }
}
