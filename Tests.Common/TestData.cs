using System;
using Core.Entities;
using Core.Entities.Checkpoints;

namespace Tests.Common;

public static class TestData
{
    public const string BunchIdA = "1";
    private const string BunchIdB = "2";
    public const string SlugA = "bunch-a";
    private const string SlugB = "bunch-b";
    private const string BunchNameA = "Bunch A";
    private const string BunchNameB = "Bunch B";
    public const string DescriptionA = "Description A";
    private const string DescriptionB = "Description B";
    public const string HouseRulesA = "House Rules A";
    private const string HouseRulesB = "House Rules B";
    public const int DefaultBuyinA = 100;
    private const int DefaultBuyinB = 200;

    public const string UserIdA = "1";
    private const string UserIdB = "2";
    private const string UserIdC = "3";
    private const string UserIdD = "4";
    public const string UserNameA = "user-name-a";
    private const string UserNameB = "user-name-b";
    public const string UserNameC = "user-name-c";
    private const string UserNameD = "user-name-d";
    public const string UserEmailA = "email-a@example.com";
    private const string UserEmailB = "email-b@example.com";
    private const string UserEmailC = "email-c@example.com";
    private const string UserEmailD = "email-d@example.com";
    private const string UserRealNameA = "Real Name A";
    private const string UserRealNameB = "Real Name B";
    private const string UserRealNameC = "Real Name C";
    private const string UserRealNameD = "Real Name D";
    public const string UserDisplayNameA = "Display Name A";
    private const string UserDisplayNameB = "Display Name B";
    private const string UserDisplayNameC = "Display Name C";
    private const string UserDisplayNameD = "Display Name D";
    public const string UserPasswordA = "PasswordA";
    public const string UserPasswordB = "PasswordB";
    private const string UserEncryptedPasswordA = "5a99a164773c45966e5fcdd1c3110937861094aa";
    private const string UserEncryptedPasswordB = "6873088c1117d25d1abf4b75272d463b0ec6a504";
    private const string UserEncryptedPasswordC = "not_used_in_any_test_yet";
    private const string UserEncryptedPasswordD = "not_used_in_any_test_yet";
    private const string UserSaltA = "SaltA";
    private const string UserSaltB = "SaltB";
    private const string UserSaltC = "SaltC";
    private const string UserSaltD = "SaltD";

    public const string LocalTimeZoneName = "W. Europe Standard Time";
    public static readonly TimeZoneInfo TimeZoneLocal = TimeZoneInfo.FindSystemTimeZoneById(LocalTimeZoneName);
    public static readonly TimeZoneInfo TimeZoneUtc = TimeZoneInfo.Utc;

    public static User UserA => new(UserIdA, UserNameA, UserDisplayNameA, UserRealNameA, UserEmailA, Role.Player, UserEncryptedPasswordA, UserSaltA);
    public static User UserB => new(UserIdB, UserNameB, UserDisplayNameB, UserRealNameB, UserEmailB, Role.Admin, UserEncryptedPasswordB, UserSaltB);
    public static User UserC => new(UserIdC, UserNameC, UserDisplayNameC, UserRealNameC, UserEmailC, Role.Player, UserEncryptedPasswordC, UserSaltC);
    public static User UserD => new(UserIdD, UserNameD, UserDisplayNameD, UserRealNameD, UserEmailD, Role.Player, UserEncryptedPasswordD, UserSaltD);
    public static User AdminUser => UserB;
    public static User ManagerUser => UserC;

    public static readonly Bunch BunchA = new(BunchIdA, SlugA, BunchNameA, DescriptionA, HouseRulesA, TimeZoneUtc, DefaultBuyinA, Currency.Default);
    public static readonly Bunch BunchB = new(BunchIdB, SlugB, BunchNameB, DescriptionB, HouseRulesB, TimeZoneLocal, DefaultBuyinB, Currency.Default);

    public static readonly Player PlayerA = new(BunchIdA, PlayerIdA, UserIdA, UserNameA, PlayerNameA, Role.Player, "#9e9e9e");
    public static readonly Player PlayerB = new(BunchIdA, PlayerIdB, UserIdB, UserNameB, PlayerNameB, Role.Player, "#9e9e9e");
    public static readonly Player PlayerC = new(BunchIdA, PlayerIdC, UserIdC, UserNameC, PlayerNameC, Role.Manager, "#9e9e9e");
    public static readonly Player PlayerD = new(BunchIdA, PlayerIdD, default, default, PlayerNameD, Role.Player, "#9e9e9e");
    public static readonly Player ManagerPlayer = PlayerC;

    public const string BuyinCheckpointId = "1";
    public const int BuyinCheckpointStack = 100;
    public const int BuyinCheckpointAmount = 200;
    public const string ReportCheckpointId = "2"; 
    public const int ReportCheckpointStack = 300;
    public const int ReportCheckpointAmount = 400;
    public const int CashoutCheckpointId = 3;
    public const int CashoutCheckpointStack = 500;
    public const int CashoutCheckpointAmount = 600;
    
    public const string PlayerIdA = "1";
    public const string PlayerIdB = "2";
    private const string PlayerIdC = "3";
    public const string PlayerIdD = "4";
    public const string PlayerNameA = "Player Name A";
    public const string PlayerNameB = "Player Name B";
    private const string PlayerNameC = "Player Name C";
    private const string PlayerNameD = "Player Name D";

    public const string CashgameIdA = "1";
    public const string CashgameIdB = "2";
    public const string CashgameIdC = "3";
    public const string LocationIdA = "1";
    public const string LocationIdB = "2";
    public const string LocationIdC = "3";
    public const string ChangedLocationId = "4";
    public const string LocationNameA = "Location A";
    public const string LocationNameB = "Location B";
    public const string LocationNameC = "Location C";
    public const string ChangedLocationName = "Changed Location";
    public const string DateStringA = "2001-01-01";
    public const string DateStringB = "2002-02-02";
    public const string DateStringC = "2003-03-03";
    public static DateTime StartTimeA = DateTime.SpecifyKind(DateTime.Parse("2001-01-01 12:00:00"), DateTimeKind.Utc);
    public static DateTime StartTimeB = DateTime.SpecifyKind(DateTime.Parse("2002-02-02 12:00:00"), DateTimeKind.Utc);
    public static DateTime StartTimeC = DateTime.SpecifyKind(DateTime.Parse("2003-03-03 12:00:00"), DateTimeKind.Utc);

    public const string EventIdA = "1";
    public const string EventIdB = "2";
    public const string EventNameA = "Event A";
    public const string EventNameB = "Event B";

    public const string TestUrl = "https://pokerbunch.com/test";

    public static IList<Checkpoint> RunningGameCheckpoints =>
        new List<Checkpoint>
        {
            Checkpoint.Create("12", CashgameIdC, PlayerIdA, StartTimeC, CheckpointType.Buyin, 200, 200),
            Checkpoint.Create("13", CashgameIdC, PlayerIdB, StartTimeC, CheckpointType.Buyin, 200, 200)
        };
}