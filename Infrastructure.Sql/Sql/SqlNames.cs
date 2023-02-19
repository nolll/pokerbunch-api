namespace Infrastructure.Sql.Sql;

public static class SqlNames
{
    public static class Location
    {
        public const string Table = "pb_location";
        
        public static class Columns
        {
            public const string Id = "location_id";
            public const string Name = "name";
            public const string BunchId = "bunch_id";
        }
    }

    public static class User
    {
        public const string Table = "pb_user";

        public static class Columns
        {
            public const string Id = "user_id";
            public const string UserName = "user_name";
            public const string DisplayName = "display_name";
            public const string RealName = "real_name";
            public const string Email = "email";
            public const string Password = "password";
            public const string Salt = "salt";
            public const string RoleId = "role_id";
        }
    }
}