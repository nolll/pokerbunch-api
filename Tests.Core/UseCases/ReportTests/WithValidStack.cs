namespace Tests.Core.UseCases.ReportTests;

public class WithValidStack : Arrange
{
    [Test]
    public void AddsCheckpoint() => UpdatedCashgame?.AddedCheckpoints.First()?.Stack.Should().Be(Stack);
}