namespace Infrastructure.Sql.Dtos;

public class JoinRequestDto
{
    public int JoinRequestId { get; set; }
    public int BunchId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
}