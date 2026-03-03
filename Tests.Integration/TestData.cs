using Tests.Common;

namespace Tests.Integration;

public class TestData(TestDataFactory dataFactory)
{
    public readonly string AdminUserName = dataFactory.String();
    public readonly string AdminDisplayName = dataFactory.String();
    public readonly string AdminEmail = dataFactory.EmailAddress();
    public readonly string AdminPassword = dataFactory.String();

    public readonly string ManagerUserName = "manager";
    public readonly string ManagerDisplayName = "Manager";
    public readonly string ManagerUserId = "2";
    public readonly string ManagerEmail = "manager@example.org";
    public readonly string ManagerPassword = "managerpassword";
    public readonly string ManagerPlayerId = "1";

    public readonly string UserUserName = "user";
    public readonly string UserDisplayName = "User";
    public readonly string UserUserId = "3";
    public readonly string UserEmail = "user@example.org";
    public readonly string UserPassword = "userpassword";
    public readonly string UserPlayerId = "2";

    public readonly string PlayerName = "Player Name";
    public readonly string PlayerPlayerId = "3";
    public readonly string TempPlayerName = "Temp player";
    public readonly string TempPlayerId = "4";

    public readonly string BunchDisplayName = "Bunch 1";
    public readonly string BunchId = "bunch-1";
    public readonly string BunchDescription = "Bunch Description 1";
    public readonly string BunchLocationId = "1";
    public readonly string BunchLocationName = "Bunch Location 1";
    public readonly string CurrencySymbol = "$";
    public readonly string CurrencyLayout = "{SYMBOL}{AMOUNT}";
    public readonly string TimeZone = "Europe/Stockholm";

    public readonly string EventName = "Event 1";
    public readonly string EventId = "1";

    public readonly string CashgameId = "1";
}