// ReSharper disable InconsistentNaming
using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

public class EventDayDto
{
    [UsedImplicitly] public string Event_Id { get; set; }
    [UsedImplicitly] public string Bunch_Id { get; set; }
    [UsedImplicitly] public string Name { get; set; }
    [UsedImplicitly] public string Location_Id { get; set; }
    [UsedImplicitly] public DateTime Timestamp { get; set; }
}