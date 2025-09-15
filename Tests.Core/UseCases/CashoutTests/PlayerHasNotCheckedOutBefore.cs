using Core.Entities.Checkpoints;

namespace Tests.Core.UseCases.CashoutTests;

public class PlayerHasNotCheckedOutBefore : Arrange
{
    [Test]
    public void AddsCheckpoint()
    {
        UpdatedCashgame!.Checkpoints.Count.Should().Be(CheckpointCountBeforeCashout + 1);
        UpdatedCashgame!.Checkpoints.First(o => o.Type == CheckpointType.Cashout).Stack.Should().Be(CashoutStack);
    }
}