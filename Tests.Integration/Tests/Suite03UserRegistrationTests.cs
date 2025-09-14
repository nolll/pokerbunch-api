using System.Net;
using Api.Models.UserModels;
using Core.Entities;
using Infrastructure.Sql.Sql;
using SqlKata;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.UserRegistration)]
public class Suite03UserRegistrationTests
{
    [Test]
    [Order(1)]
    public async Task Test01RegisterAdminReturns200()
    {
        var parameters = new AddUserPostModel(TestData.AdminUserName, TestData.AdminDisplayName, TestData.AdminEmail, TestData.AdminPassword);
        var result = await TestClient.User.Add(parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        await SetAdminRole();
    }

    private static async Task SetAdminRole()
    {
        var dbParameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.User.RoleId, (int)Role.Admin }
        };

        var query = new Query(Schema.User).Where(Schema.User.Id, int.Parse(TestData.AdminUserId));
        await TestSetup.Db.UpdateAsync(query, dbParameters);
    }
    
    [Test]
    [Order(2)]
    public async Task Test02RegisterManagerReturns200()
    {
        var parameters = new AddUserPostModel(TestData.ManagerUserName, TestData.ManagerDisplayName, TestData.ManagerEmail, TestData.ManagerPassword);
        var result = await TestClient.User.Add(parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    [Order(3)]
    public async Task Test03RegisterRegularUserReturns200()
    {
        var parameters = new AddUserPostModel(TestData.UserUserName, TestData.UserDisplayName, TestData.UserEmail, TestData.UserPassword);
        var result = await TestClient.User.Add(parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    }