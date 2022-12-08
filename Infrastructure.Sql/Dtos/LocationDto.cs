using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

public class LocationDto
{
    [UsedImplicitly] public string Location_Id { get; set; }
    [UsedImplicitly] public string Name { get; set; }
    [UsedImplicitly] public string Bunch_Id { get; set; }
}