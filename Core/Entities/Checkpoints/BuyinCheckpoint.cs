using System;

namespace Core.Entities.Checkpoints;

public class BuyinCheckpoint : Checkpoint
{
    public override string Description => "Buyin";

    public BuyinCheckpoint(int cashgameId, int playerId, DateTime timestamp, int stack, int amount, int id = 0)
        : base(cashgameId, playerId, timestamp, CheckpointType.Buyin, stack, amount, id)
    {
    }
}