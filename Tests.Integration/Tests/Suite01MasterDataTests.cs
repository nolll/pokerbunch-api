using Microsoft.EntityFrameworkCore;

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
        var query = TestSetup.Db!.PbRole.OrderBy(o => o.RoleId);
        var roles = await query.ToListAsync();
        
        roles.Count.Should().Be(3);
        roles[0].RoleId.Should().Be(1);
        roles[0].RoleName.Should().Be("Player");
        roles[1].RoleId.Should().Be(2);
        roles[1].RoleName.Should().Be("Manager");
        roles[2].RoleId.Should().Be(3);
        roles[2].RoleName.Should().Be("Admin");
    }
}