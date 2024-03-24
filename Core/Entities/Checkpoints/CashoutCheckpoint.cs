using System;

namespace Core.Entities.Checkpoints;

public class CashoutCheckpoint(
    string cashgameId,
    string playerId,
    DateTime timestamp,
    int stack,
    int amount,
    string? id)
    : Checkpoint(cashgameId, playerId, timestamp, CheckpointType.Cashout, stack, amount, id)
{
    public override string Description => "Cashout";
}