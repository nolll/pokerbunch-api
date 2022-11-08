using Infrastructure.Sql.Interfaces;

namespace Tests.Integration;

internal class RoleInTest
{
    private RoleInTest(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static RoleInTest Create(IStorageDataReader reader)
    {
        return new RoleInTest(
            reader.GetIntValue("role_id"),
            reader.GetStringValue("role_name"));
    }

    public int Id { get; }
    public string Name { get; }
}