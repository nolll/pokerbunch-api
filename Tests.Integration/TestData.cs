namespace Tests.Integration;

public static class TestData
{
    public const string AdminUserId = "1";
    public const string AdminUserName = "admin";
    public const string AdminDisplayName = "Admin";
    public const string AdminEmail = "admin@example.org";
    public const string AdminPassword = "adminpassword";

    public const string ManagerUserName = "manager";
    public const string ManagerDisplayName = "Manager";
    public const string ManagerUserId = "2";
    public const string ManagerEmail = "manager@example.org";
    public const string ManagerPassword = "managerpassword";
    public const string ManagerPlayerId = "1";

    public const string UserUserName = "user";
    public const string UserDisplayName = "User";
    public const string UserUserId = "3";
    public const string UserPlayerName = "User Player Name";
    public const string UserEmail = "user@example.org";
    public const string UserPassword = "userpassword";
    public const string UserPlayerId = "2";

    public const string PlayerName = "Player Name";
    public const string PlayerPlayerId = "3";

    public const string BunchDisplayName = "Bunch 1";
    public const string BunchId = "bunch-1";
    public const string BunchDescription = "Bunch Description 1";
    public const string BunchLocationId = "1";
    public const string BunchLocationName = "Bunch Location 1";
    public const string CurrencySymbol = "$";
    public const string CurrencyLayout = "{SYMBOL}{AMOUNT}";
    public const string TimeZone = "Europe/Stockholm";

    public const string EventName = "Event 1";
    public const string EventId = "1";

    public const string CashgameId = "1";

    public static string? AdminToken { get; set; }
    public static string? ManagerToken { get; set; }
    public static string? UserToken { get; set; }
}