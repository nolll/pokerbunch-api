using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.CurrentCashgamesTests
{
    public class WithoutGames : Arrange
    {
        protected override Role Role => Role.Player;

        [Test]
        public void ReturnsEmptyList() => Assert.AreEqual(0, Result.Games.Count);
    }
}
