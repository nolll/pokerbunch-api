namespace Tests.Core.UseCases.CurrentCashgamesTests;

public class HasAccessWithGames : Arrange
{
    protected override bool CanListCurrentGames => true;
    protected override int GameCount => 1;

    [Test]
    public void ReturnsListOfGames()
    {
        var games = Result!.Data!.Games;
        games!.Count.Should().Be(1);
        var game = games.First();
        game.Slug.Should().Be(Slug);
        game.Id.Should().Be(CashgameId);
    }
}