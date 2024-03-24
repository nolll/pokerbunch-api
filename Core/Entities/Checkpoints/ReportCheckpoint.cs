using System;

namespace Core.Entities.Checkpoints;

public class ReportCheckpoint(string cashgameId, string playerId, DateTime timestamp, int stack, int amount, string? id)
    : Checkpoint(cashgameId, playerId, timestamp, CheckpointType.Report, stack, amount, id)
{
    public override string Description => "Report";
}