using Core.Entities.Checkpoints;

namespace Tests.Core.UseCases.CashoutTests;

public class PlayerHasNotCheckedOutBefore : Arrange
{
    [Test]
    public void AddsCheckpoint()
    {
        Assert.That(UpdatedCashgame?.Checkpoints.Count, Is.EqualTo(CheckpointCountBeforeCashout + 1));
        Assert.That(UpdatedCashgame?.Checkpoints.First(o => o.Type == CheckpointType.Cashout).Stack, Is.EqualTo(CashoutStack));
    }
}