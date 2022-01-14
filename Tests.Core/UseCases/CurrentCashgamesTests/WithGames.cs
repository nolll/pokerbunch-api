using System.Linq;
using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.CurrentCashgamesTests;

public class WithGames : Arrange
{
    protected override Role Role => Role.Player;
    protected override int GameCount => 1;

    [Test]
    public void ReturnsListOfGames()
    {
        var games = Result.Games;
        Assert.AreEqual(1, games.Count);
        var game = games.First();
        Assert.AreEqual(Slug, game.Slug);
        Assert.AreEqual(CashgameId, game.Id);
    }
}