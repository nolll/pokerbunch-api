namespace Tests.Integration;

public static class TestOrder
{
    public const int MasterData = 1;
    public const int General = 2;
    public const int UserRegistration = 3;
    public const int Login = 4;
    public const int User = 5;
    public const int Admin = 6;
    public const int Bunch = 7;
    public const int Location = 8;
    public const int CashgamePlay = 9;
    public const int Event = 10;
    public const int Player = 11;
}

/* ---- Routes left to test ----
Action.Update
Action.Delete
Action.List
Bunch.Update
Bunch.List
Bunch.ListForCurrentUser
Cashgame.ListByBunch
Cashgame.Update
Cashgame.Delete
Cashgame.ListByBunchAndYear
Cashgame.ListCurrentByBunch
Cashgame.ListByEvent
Cashgame.ListByPlayer
Cashgame.YearsByBunch
Profile.Get
Profile.ChangePassword
Profile.ResetPassword
Auth.Token
User.Update
*/

/* ---- Routes with tests ----
Error (no tests needed)
Root
Settings
Version
Action.Add
Admin.ClearCache
Admin.SendEmail
Auth.Login
Bunch.Add
Bunch.Get
Bunch.Join
Cashgame.Add
Cashgame.Get
Event.Add
Event.Get
Event.ListByBunch
Location.Get
Location.ListByBunch
Location.Add
Player.Add
Player.Delete
Player.Get
Player.Invite
Player.ListByBunch
User.Add
User.Get
User.List
*/