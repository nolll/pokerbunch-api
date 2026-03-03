using System.Net;
using Api.Models.UserModels;
using Core.Entities;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.UserRegistration, 1)]
    public async Task Suite03UserRegistration_01RegisterAdminReturns200()
    {
        var parameters = new AddUserPostModel(TestData.AdminUserName, TestData.AdminDisplayName, TestData.AdminEmail, TestData.AdminPassword);
        var result = await fixture.ApiClient.User.Add(parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        await SetAdminRole();
    }
    
    [Fact]
    [Order(TestSuite.UserRegistration, 2)]
    public async Task Suite03UserRegistration_02RegisterManagerReturns200()
    {
        var parameters = new AddUserPostModel(TestData.ManagerUserName, TestData.ManagerDisplayName, TestData.ManagerEmail, TestData.ManagerPassword);
        var result = await fixture.ApiClient.User.Add(parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.UserRegistration, 3)]
    public async Task Suite03UserRegistration_03RegisterRegularUserReturns200()
    {
        var parameters = new AddUserPostModel(TestData.UserUserName, TestData.UserDisplayName, TestData.UserEmail, TestData.UserPassword);
        var result = await fixture.ApiClient.User.Add(parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    private async Task SetAdminRole()
    {
        var admin = fixture.Db!.PbUser
            .First(o => o.UserName == TestData.AdminUserName);

        admin.RoleId = (int)Role.Admin;
        await fixture.Db.SaveChangesAsync();
    }
}