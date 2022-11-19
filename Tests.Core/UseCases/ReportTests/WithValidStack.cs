namespace Tests.Core.UseCases.ReportTests;

public class WithValidStack : Arrange
{
    [Test]
    public void AddsCheckpoint()
    {
        var addedCheckpoint = UpdatedCashgame.AddedCheckpoints.First();
        Assert.AreEqual(Stack, addedCheckpoint.Stack);
    }
}