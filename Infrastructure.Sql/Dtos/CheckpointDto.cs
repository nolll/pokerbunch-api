namespace Infrastructure.Sql.Dtos;

public class CheckpointDto
{
    public int CashgameId { get; set; }
    public int PlayerId { get; set; }
    public int Amount { get; set; }
    public int Stack { get; set; }
    public DateTime Timestamp { get; set; }
    public int CheckpointId { get; set; }
    public int Type { get; set; }
}