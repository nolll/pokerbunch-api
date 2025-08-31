namespace Tests.Core.UseCases.CurrentCashgamesTests;

public class HasAccessWithoutGames : Arrange
{
    protected override bool CanListCurrentGames => true;

    [Test]
    public void ReturnsEmptyList() => Assert.That(Result?.Data?.Games.Count, Is.EqualTo(0));
}