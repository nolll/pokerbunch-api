using System;

namespace Core.Entities.Checkpoints
{
    public class CashoutCheckpoint : Checkpoint
    {
        public override string Description => "Cashout";

        public CashoutCheckpoint(int cashgameId, int playerId, DateTime timestamp, int stack, int amount, int id)
            : base(cashgameId, playerId, timestamp, CheckpointType.Cashout, stack, amount, id)
        {
        }
    }
}