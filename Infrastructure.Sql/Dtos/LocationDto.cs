namespace Infrastructure.Sql.Dtos;

public class LocationDto
{
    public int LocationId { get; set; }
    public string Name { get; set; } = "";
    public string BunchSlug { get; set; } = "";
}