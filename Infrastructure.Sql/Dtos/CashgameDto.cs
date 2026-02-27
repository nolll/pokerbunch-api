namespace Infrastructure.Sql.Dtos;

public class CashgameDto
{
    public int CashgameId { get; set; }
    public string BunchSlug { get; set; } = "";
    public int LocationId { get; set; }
    public int? EventId { get; set; }
    public int Status { get; set; }
}