using Infrastructure.Sql.Sql;
using SqlKata;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.MasterData)]
public class Suite01MasterDataTests
{
    [Test]
    [Order(1)]
    public async Task Test01MasterDataExists()
    {
        var query = new Query(Schema.Role)
            .Select(Schema.Role.Id, Schema.Role.Name)
            .OrderBy(Schema.Role.Id);

        var roles = (await TestSetup.Db.GetAsync<RoleInTest>(query)).ToList();

        roles.Count.Should().Be(3);
        roles[0].Role_Id.Should().Be(1);
        roles[0].Role_Name.Should().Be("Player");
        roles[1].Role_Id.Should().Be(2);
        roles[1].Role_Name.Should().Be("Manager");
        roles[2].Role_Id.Should().Be(3);
        roles[2].Role_Name.Should().Be("Admin");
    }
}