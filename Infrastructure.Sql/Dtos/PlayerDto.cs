namespace Infrastructure.Sql.Dtos;

public class PlayerDto
{
    public string BunchSlug { get; set; } = "";
    public int PlayerId { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; } = "";
    public string PlayerName { get; set; } = "";
    public int RoleId { get; set; }
    public string? Color { get; set; }
}