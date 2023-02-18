// ReSharper disable InconsistentNaming
using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

public class LocationDto
{
    [UsedImplicitly] public int Location_Id { get; set; }
    [UsedImplicitly] public string Name { get; set; } = "";
    [UsedImplicitly] public int Bunch_Id { get; set; }
}