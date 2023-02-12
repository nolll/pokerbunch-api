using System;

namespace Core.Entities.Checkpoints;

public class BuyinCheckpoint : Checkpoint
{
    public override string Description => "Buyin";

    public BuyinCheckpoint(string cashgameId, string playerId, DateTime timestamp, int stack, int amount, string? id)
        : base(cashgameId, playerId, timestamp, CheckpointType.Buyin, stack, amount, id)
    {
    }
}