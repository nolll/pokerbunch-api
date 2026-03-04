using Tests.Common;

namespace Tests.Integration;

public class TestData(TestDataFactory dataFactory)
{
    public readonly string AdminUserName = dataFactory.String();
    public readonly string AdminDisplayName = dataFactory.String();
    public readonly string AdminEmail = dataFactory.EmailAddress();
    public readonly string AdminPassword = dataFactory.String();

    public readonly string ManagerUserName = dataFactory.String();
    public readonly string ManagerDisplayName = dataFactory.String();
    public readonly string ManagerUserId = "2";
    public readonly string ManagerEmail = dataFactory.EmailAddress();
    public readonly string ManagerPassword = dataFactory.String();
    public readonly string ManagerPlayerId = "1";

    public readonly string UserUserName = dataFactory.String();
    public readonly string UserDisplayName = dataFactory.String();
    public readonly string UserUserId = "3";
    public readonly string UserEmail = dataFactory.EmailAddress();
    public readonly string UserPassword = dataFactory.String();
    public readonly string UserPlayerId = "2";

    public readonly string PlayerName = dataFactory.String();
    public readonly string PlayerPlayerId = "3";
    
    public readonly string TempPlayerName = dataFactory.String();
    public readonly string TempPlayerId = "4";

    public readonly string BunchDisplayName = "Bunch 1";
    public readonly string BunchId = "bunch-1";
    public readonly string BunchDescription = dataFactory.String();
    public readonly string BunchLocationId = "1";
    public readonly string BunchLocationName = dataFactory.String();
    public readonly string CurrencySymbol = "$";
    public readonly string CurrencyLayout = "{SYMBOL}{AMOUNT}";
    public readonly string TimeZone = "Europe/Stockholm";

    public readonly string EventName = dataFactory.String();
    public readonly string EventId = "1";

    public readonly string CashgameId = "1";
}