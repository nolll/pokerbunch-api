using Tests.Common;

namespace Tests.Integration;

public class TestData(TestDataFactory dataFactory)
{
    public readonly string AdminUserName = dataFactory.String();
    public readonly string AdminDisplayName = dataFactory.String();
    public readonly string AdminPassword = dataFactory.String();

    public readonly string ManagerUserName = dataFactory.String();
    public readonly string ManagerDisplayName = dataFactory.String();
    public readonly string ManagerPassword = dataFactory.String();

    public readonly string UserUserName = dataFactory.String();
    public readonly string UserDisplayName = dataFactory.String();
    public readonly string UserEmail = dataFactory.EmailAddress();
    public readonly string UserPassword = dataFactory.String();
}