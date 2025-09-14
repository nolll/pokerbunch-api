namespace Tests.Core.UseCases.CurrentCashgamesTests;

public class HasAccessWithoutGames : Arrange
{
    protected override bool CanListCurrentGames => true;

    [Test]
    public void ReturnsEmptyList() => Result?.Data?.Games.Count.Should().Be(0);
}