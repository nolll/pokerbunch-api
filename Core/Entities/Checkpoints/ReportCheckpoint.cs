using System;

namespace Core.Entities.Checkpoints
{
    public class ReportCheckpoint : Checkpoint
    {
        public override string Description => "Report";

        public ReportCheckpoint(int cashgameId, int playerId, DateTime timestamp, int stack, int amount, int id)
            : base(cashgameId, playerId, timestamp, CheckpointType.Report, stack, amount, id)
        {
        }
    }
}