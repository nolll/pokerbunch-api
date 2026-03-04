using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.MasterData, 1)]
    public async Task Suite01MasterData_01MasterDataExists()
    {
        var query = Db.PbRole.OrderBy(o => o.RoleId);
        var roles = await query.ToListAsync(CancellationToken.None);
        
        roles.Count.Should().Be(3);
        roles[0].RoleId.Should().Be((int)Role.Player);
        roles[0].RoleName.Should().Be(nameof(Role.Player));
        roles[1].RoleId.Should().Be((int)Role.Manager);
        roles[1].RoleName.Should().Be(nameof(Role.Manager));
        roles[2].RoleId.Should().Be((int)Role.Admin);
        roles[2].RoleName.Should().Be(nameof(Role.Admin));
    }
}