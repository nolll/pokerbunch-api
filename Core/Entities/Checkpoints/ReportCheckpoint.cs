using System;

namespace Core.Entities.Checkpoints;

public class ReportCheckpoint : Checkpoint
{
    public override string Description => "Report";

    public ReportCheckpoint(string cashgameId, string playerId, DateTime timestamp, int stack, int amount, string id = null)
        : base(cashgameId, playerId, timestamp, CheckpointType.Report, stack, amount, id)
    {
    }
}