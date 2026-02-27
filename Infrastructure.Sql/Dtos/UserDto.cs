namespace Infrastructure.Sql.Dtos;

public class UserDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string? DisplayName { get; set; }
    public string? RealName { get; set; }
    public string? Email { get; set; }
    public int RoleId { get; set; }
    public string? Password { get; set; }
    public string? Salt { get; set; }
}