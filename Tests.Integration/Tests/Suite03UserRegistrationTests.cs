using System.Net;
using Api.Models.UserModels;
using Core.Entities;

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
        var admin = TestSetup.Db!.PbUser
            .First(o => o.UserName == TestData.AdminUserName);

        admin.RoleId = (int)Role.Admin;
        await TestSetup.Db.SaveChangesAsync();
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