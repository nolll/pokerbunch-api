namespace Infrastructure.Sql.Dtos;

public class BunchDto
{
    public int BunchId { get; set; }
    public string Name { get; set; } = "";
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? HouseRules { get; set; }
    public string? Timezone { get; set; }
    public int DefaultBuyin { get; set; }
    public string? CurrencyLayout { get; set; }
    public string? Currency { get; set; }
    public bool CashgamesEnabled { get; set; }
    public bool TournamentsEnabled { get; set; }
    public bool VideosEnabled { get; set; }
}
