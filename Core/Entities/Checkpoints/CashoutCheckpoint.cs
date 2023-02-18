using System;

namespace Core.Entities.Checkpoints;

public class CashoutCheckpoint : Checkpoint
{
    public override string Description => "Cashout";

    public CashoutCheckpoint(string cashgameId, string playerId, DateTime timestamp, int stack, int amount, string? id)
        : base(cashgameId, playerId, timestamp, CheckpointType.Cashout, stack, amount, id)
    {
    }
}