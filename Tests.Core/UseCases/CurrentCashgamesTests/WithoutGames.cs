using Core.Entities;

namespace Tests.Core.UseCases.CurrentCashgamesTests;

public class WithoutGames : Arrange
{
    protected override Role Role => Role.Player;

    [Test]
    public void ReturnsEmptyList() => Assert.That(Result.Data.Games.Count, Is.EqualTo(0));
}