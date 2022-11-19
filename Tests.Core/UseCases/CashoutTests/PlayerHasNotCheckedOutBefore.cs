using System.Linq;
using Core.Entities.Checkpoints;

namespace Tests.Core.UseCases.CashoutTests;

public class PlayerHasNotCheckedOutBefore : Arrange
{
    [Test]
    public void AddsCheckpoint()
    {
        Assert.AreEqual(CheckpointCountBeforeCashout + 1, UpdatedCashgame.Checkpoints.Count);
        Assert.IsTrue(UpdatedCashgame.Checkpoints.First(o => o.Type == CheckpointType.Cashout).Stack == CashoutStack);
    }
}