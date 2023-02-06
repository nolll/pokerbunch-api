using Core.Entities;

namespace Tests.Core.UseCases.CurrentCashgamesTests;

public class WithGames : Arrange
{
    protected override Role Role => Role.Player;
    protected override int GameCount => 1;

    [Test]
    public void ReturnsListOfGames()
    {
        var games = Result?.Data.Games;
        Assert.That(games?.Count, Is.EqualTo(1));
        var game = games?.First();
        Assert.That(game?.Slug, Is.EqualTo(Slug));
        Assert.That(game?.Id, Is.EqualTo(CashgameId));
    }
}