using Core.Entities.Checkpoints;

namespace Tests.Core.UseCases.CashoutTests;

public class PlayerHasCheckedOutBefore : Arrange
{
    protected override bool HasCashedOutBefore => true;

    [Test]
    public void UpdatesCheckpoint()
    {
        Assert.That(UpdatedCashgame?.Checkpoints.Count, Is.EqualTo(CheckpointCountBeforeCashout));
        Assert.That(UpdatedCashgame?.UpdatedCheckpoints.First(o => o.Type == CheckpointType.Cashout).Stack, Is.EqualTo(CashoutStack));
    }
}