// ReSharper disable InconsistentNaming
using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

public class CashgameDto
{
    [UsedImplicitly] public int Cashgame_Id { get; set; }
    [UsedImplicitly] public int Bunch_Id { get; set; }
    [UsedImplicitly] public int Location_Id { get; set; }
    [UsedImplicitly] public int Event_Id { get; set; }
    [UsedImplicitly] public int Status { get; set; }
}