using Infrastructure.Sql.Interfaces;

namespace Tests.Integration;

internal class UserInTest
{
    private UserInTest(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static UserInTest Create(IStorageDataReader reader)
    {
        return new UserInTest(
            reader.GetIntValue("user_id"),
            reader.GetStringValue("user_name"));
    }

    public int Id { get; }
    public string Name { get; }
}