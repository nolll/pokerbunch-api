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
        var parameters = new AddUserPostModel(Data.AdminUserName, Data.AdminDisplayName, Data.AdminEmail, Data.AdminPassword);
        var result = await ApiClient.User.Add(parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        await SetAdminRole();
    }
    
    [Fact]
    [Order(TestSuite.UserRegistration, 2)]
    public async Task Suite03UserRegistration_02RegisterManagerReturns200()
    {
        var parameters = new AddUserPostModel(Data.ManagerUserName, Data.ManagerDisplayName, Data.ManagerEmail, Data.ManagerPassword);
        var result = await ApiClient.User.Add(parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.UserRegistration, 3)]
    public async Task Suite03UserRegistration_03RegisterRegularUserReturns200()
    {
        var parameters = new AddUserPostModel(Data.UserUserName, Data.UserDisplayName, Data.UserEmail, Data.UserPassword);
        var result = await ApiClient.User.Add(parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    private async Task SetAdminRole()
    {
        var admin = Db.PbUser
            .First(o => o.UserName == Data.AdminUserName);

        admin.RoleId = (int)Role.Admin;
        await Db.SaveChangesAsync();
    }
}