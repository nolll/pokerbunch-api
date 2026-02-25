namespace Infrastructure.Sql.Sql;

public static class Schema
{
    public static readonly SqlBunch Bunch = new();
    public static readonly SqlCashgame Cashgame = new();
    public static readonly SqlCashgameCheckpoint CashgameCheckpoint = new();
    public static readonly SqlEventCashgame EventCashgame = new();
    public static readonly SqlRole Role = new();
    public static readonly SqlUser User = new();
}