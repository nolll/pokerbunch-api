using JetBrains.Annotations;

namespace Infrastructure.Sql.Classes;

public class RawLocation
{
    [UsedImplicitly] public string Location_Id { get; set; }
    [UsedImplicitly] public string Name { get; set; }
    [UsedImplicitly] public string BunchId { get; set; }
}