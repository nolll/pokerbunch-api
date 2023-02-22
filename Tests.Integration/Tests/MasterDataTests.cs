using Infrastructure.Sql.Sql;
using SqlKata;
using SqlKata.Execution;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.MasterData)]
public class MasterDataTests
{
    [Test]
    [Order(1)]
    public async Task MasterDataExists()
    {
        var query = new Query(Schema.Role)
            .Select(Schema.Role.Id, Schema.Role.Name)
            .OrderBy(Schema.Role.Id);

        var roles = (await TestSetup.Db.QueryFactory.FromQuery(query).GetAsync<RoleInTest>()).ToList();

        Assert.That(roles.Count, Is.EqualTo(3));
        Assert.That(roles[0].Role_Id, Is.EqualTo(1));
        Assert.That(roles[0].Role_Name, Is.EqualTo("Player"));
        Assert.That(roles[1].Role_Id, Is.EqualTo(2));
        Assert.That(roles[1].Role_Name, Is.EqualTo("Manager"));
        Assert.That(roles[2].Role_Id, Is.EqualTo(3));
        Assert.That(roles[2].Role_Name, Is.EqualTo("Admin"));
    }
}