using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.MasterData, 1)]
    public async Task Suite01MasterData_01MasterDataExists()
    {
        var query = fixture.Db!.PbRole.OrderBy(o => o.RoleId);
        var roles = await query.ToListAsync(CancellationToken.None);
        
        roles.Count.Should().Be(3);
        roles[0].RoleId.Should().Be(1);
        roles[0].RoleName.Should().Be("Player");
        roles[1].RoleId.Should().Be(2);
        roles[1].RoleName.Should().Be("Manager");
        roles[2].RoleId.Should().Be(3);
        roles[2].RoleName.Should().Be("Admin");
    }
}