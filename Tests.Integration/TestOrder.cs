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
    public const int Event = 9;
    public const int Player = 10;
    public const int Cashgame = 11;
}

/* ---- Routes left to test ----
Cashgame.Delete
Profile.ChangePassword
Profile.ResetPassword
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
Profile.Get
User.Add
User.Get
User.List
*/