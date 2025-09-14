using Core.Entities.Checkpoints;

namespace Tests.Core.UseCases.CashoutTests;

public class PlayerHasCheckedOutBefore : Arrange
{
    protected override bool HasCashedOutBefore => true;

    [Test]
    public void UpdatesCheckpoint()
    {
        UpdatedCashgame?.Checkpoints.Count.Should().Be(CheckpointCountBeforeCashout);
        UpdatedCashgame?.UpdatedCheckpoints.First(o => o.Type == CheckpointType.Cashout).Stack.Should().Be(CashoutStack);
    }
}