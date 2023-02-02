// ReSharper disable InconsistentNaming
using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

public class BunchDto
{
    [UsedImplicitly] public int Bunch_Id { get; set; }
    [UsedImplicitly] public string Name { get; set; }
    [UsedImplicitly] public string Display_Name { get; set; }
    [UsedImplicitly] public string Description { get; set; }
    [UsedImplicitly] public string House_Rules { get; set; }
    [UsedImplicitly] public string Timezone { get; set; }
    [UsedImplicitly] public int Default_Buyin { get; set; }
    [UsedImplicitly] public string Currency_Layout { get; set; }
    [UsedImplicitly] public string Currency { get; set; }
    [UsedImplicitly] public bool Cashgames_Enabled { get; set; }
    [UsedImplicitly] public bool Tournaments_Enabled { get; set; }
    [UsedImplicitly] public bool Videos_Enabled { get; set; }
}
