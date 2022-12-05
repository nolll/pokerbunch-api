using Core.Entities;
using JetBrains.Annotations;

namespace Infrastructure.Sql.Classes;

public class RawBunch
{
    [UsedImplicitly] public string Bunch_Id { get; set; }
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

    public static RawBunch Create(Bunch bunch)
    {
        return new RawBunch
        {
            Bunch_Id = bunch.Id,
            Name = bunch.Slug,
            Display_Name = bunch.DisplayName,
            Description = bunch.Description,
            House_Rules = bunch.HouseRules,
            Timezone = bunch.Timezone.Id,
            Default_Buyin = bunch.DefaultBuyin,
            Currency_Layout = bunch.Currency.Layout,
            Currency = bunch.Currency.Symbol,
            Cashgames_Enabled = bunch.CashgamesEnabled,
            Tournaments_Enabled = bunch.TournamentsEnabled,
            Videos_Enabled = bunch.VideosEnabled
        };
    }
}