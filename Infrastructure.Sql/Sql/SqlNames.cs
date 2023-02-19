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
}