using Core.Entities.Checkpoints;
using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

public class CheckpointDto
{
    [UsedImplicitly] public int Cashgame_Id { get; set; }
    [UsedImplicitly] public int Player_Id { get; set; }
    [UsedImplicitly] public int Amount { get; set; }
    [UsedImplicitly] public int Stack { get; set; }
    [UsedImplicitly] public DateTime Timestamp { get; set; }
    [UsedImplicitly] public int Checkpoint_Id { get; set; }
    [UsedImplicitly] public int Type { get; set; }
    
    public static Checkpoint CreateReal(CheckpointDto checkpointDto)
    {
        return Checkpoint.Create(
            checkpointDto.Cashgame_Id.ToString(),
            checkpointDto.Player_Id.ToString(),
            TimeZoneInfo.ConvertTimeToUtc(checkpointDto.Timestamp),
            (CheckpointType)checkpointDto.Type,
            checkpointDto.Stack,
            checkpointDto.Amount,
            checkpointDto.Checkpoint_Id.ToString());
    }
}