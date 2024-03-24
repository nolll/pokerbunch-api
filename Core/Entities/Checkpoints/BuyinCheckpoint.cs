using System;

namespace Core.Entities.Checkpoints;

public class BuyinCheckpoint(string cashgameId, string playerId, DateTime timestamp, int stack, int amount, string? id)
    : Checkpoint(cashgameId, playerId, timestamp, CheckpointType.Buyin, stack, amount, id)
{
    public override string Description => "Buyin";
}