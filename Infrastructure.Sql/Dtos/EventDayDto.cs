// ReSharper disable InconsistentNaming
using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

public class EventDayDto
{
    [UsedImplicitly] public int Event_Id { get; set; }
    [UsedImplicitly] public string Bunch_Slug { get; set; } = "";
    [UsedImplicitly] public string Name { get; set; } = "";
    [UsedImplicitly] public int? Location_Id { get; set; }
    [UsedImplicitly] public DateTime Timestamp { get; set; }
}