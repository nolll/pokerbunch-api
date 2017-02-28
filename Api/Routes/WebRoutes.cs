namespace Api.Routes
{
    public static class WebRoutes
    {
        public const string Home = "";

        public static class Auth
        {
            public const string Login = "auth/login";
            public const string Logout = "auth/logout";
        }

        public static class Cashgame
        {
            public const string Add = "cashgame/add/{slug}";
            public const string Chart = "cashgame/chart/{slug}";
            public const string ChartWithYear = Chart + "/{year?}";
            public const string Delete = "cashgame/delete/{id}";
            public const string Details = "cashgame/details/{id}";
            public const string Edit = "cashgame/edit/{id}";
            public const string End = "cashgame/end/{slug}";
            public const string Index = "cashgame/index/{slug}";
            public const string Toplist = "cashgame/toplist/{slug}";
            public const string ToplistWithYear = Toplist + "/{year?}";
            public const string Matrix = "cashgame/matrix/{slug}";
            public const string MatrixWithYear = Matrix + "/{year?}";
            public const string List = "cashgame/list/{slug}";
            public const string ListWithYear = List + "/{year?}";
            public const string Facts = "cashgame/facts/{slug}";
            public const string FactsWithYear = Facts + "/{year?}";
            public const string Action = "cashgame/action/{cashgameId}/{playerId}";
            public const string Buyin = "cashgame/buyin/{slug}";
            public const string Report = "cashgame/report/{slug}";
            public const string Cashout = "cashgame/cashout/{slug}";
            public const string Running = "cashgame/running/{slug}";
            public const string Dashboard = "cashgame/dashboard/{slug}";
            public const string RunningGameJson = "cashgame/runninggamejson/{slug}";
            public const string RunningPlayersJson = "cashgame/runningplayersjson/{slug}";
            public const string CheckpointDelete = "cashgame/deletecheckpoint/{id}";
            public const string CheckpointEdit = "cashgame/editcheckpoint/{id}";
        }

        public static class Bunch
        {
            public const string Add = "bunch/add";
            public const string AddConfirmation = "bunch/created";
            public const string Details = "bunch/details/{slug}";
            public const string Edit = "bunch/edit/{slug}";
            public const string Join = "bunch/join/{slug}";
            public const string JoinWithCode = "bunch/join/{slug}/{code}";
            public const string JoinConfirmation = "bunch/joined/{slug}";
            public const string All = "bunch/all";
        }

        public static class Event
        {
            public const string List = "event/list/{slug}";
            public const string Details = "event/details/{id}";
            public const string Add = "event/add/{slug}";
            public const string AddConfirmation = "event/created/{slug}";
        }

        public static class Location
        {
            public const string List = "location/list/{slug}";
            public const string Details = "location/details/{id}";
            public const string Edit = "location/edit/{id}";
            public const string Add = "location/add/{slug}";
            public const string AddConfirmation = "location/created/{slug}";
        }

        public static class Player
        {
            public const string Add = "player/add/{slug}";
            public const string AddConfirmation = "player/created/{slug}";
            public const string Delete = "player/delete/{id}";
            public const string Details = "player/details/{id}";
            public const string List = "player/list/{slug}";
            public const string Invite = "player/invite/{id}";
            public const string InviteConfirmation = "player/invited/{id}";
        }

        public static class User
        {
            public const string Add = "user/add";
            public const string AddConfirmation = "user/created";
            public const string Details = "user/details/{userName}";
            public const string Edit = "user/edit/{userName}";
            public const string List = "user/list";
            public const string ChangePassword = "user/changepassword";
            public const string ChangePasswordConfirmation = "user/changedpassword";
            public const string ForgotPassword = "user/forgotpassword";
            public const string ForgotPasswordConfirmation = "user/passwordsent";
        }

        public static class Api
        {
            public const string Docs = "api";
        }

        public static class App
        {
            public const string List = "apps/list";
            public const string All = "apps/all";
            public const string Details = "apps/details/{id}";
            public const string Edit = "apps/edit/{id}";
            public const string Add = "apps/add";
            public const string AddConfirmation = "apps/added";
        }
    }
}