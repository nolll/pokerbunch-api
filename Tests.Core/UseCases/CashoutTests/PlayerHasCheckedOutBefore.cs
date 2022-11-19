﻿using Core.Entities.Checkpoints;

namespace Tests.Core.UseCases.CashoutTests;

public class PlayerHasCheckedOutBefore : Arrange
{
    protected override bool HasCashedOutBefore => true;

    [Test]
    public void UpdatesCheckpoint()
    {
        Assert.AreEqual(CheckpointCountBeforeCashout, UpdatedCashgame.Checkpoints.Count);
        Assert.IsTrue(UpdatedCashgame.UpdatedCheckpoints.First(o => o.Type == CheckpointType.Cashout).Stack == CashoutStack);
    }
}