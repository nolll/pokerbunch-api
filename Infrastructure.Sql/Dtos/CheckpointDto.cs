// ReSharper disable InconsistentNaming
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
}