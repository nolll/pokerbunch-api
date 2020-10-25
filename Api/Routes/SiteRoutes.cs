namespace Api.Routes
{
    public static class SiteRoutes
    {
        public const string AddUser = "user/add";
        public const string JoinBunch = "bunch/join/{bunchId}";
        public const string JoinBunchWithCode = "bunch/join/{bunchId}/{code}";
        public const string ApiDocs = "apidocs";
        public const string Login = "auth/login";
    }
}