using Core.Entities.Checkpoints;
using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

public class RawCheckpoint
{
    [UsedImplicitly] public int Cashgame_Id { get; set; }
    [UsedImplicitly] public int Player_Id { get; set; }
    [UsedImplicitly] public int Amount { get; set; }
    [UsedImplicitly] public int Stack { get; set; }
    [UsedImplicitly] public DateTime Timestamp { get; set; }
    [UsedImplicitly] public int Checkpoint_Id { get; set; }
    [UsedImplicitly] public int Type { get; set; }
    
    public static Checkpoint CreateReal(RawCheckpoint rawCheckpoint)
    {
        return Checkpoint.Create(
            rawCheckpoint.Cashgame_Id.ToString(),
            rawCheckpoint.Player_Id.ToString(),
            TimeZoneInfo.ConvertTimeToUtc(rawCheckpoint.Timestamp),
            (CheckpointType)rawCheckpoint.Type,
            rawCheckpoint.Stack,
            rawCheckpoint.Amount,
            rawCheckpoint.Checkpoint_Id.ToString());
    }
}