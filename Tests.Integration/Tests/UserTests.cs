using System.Net;
using System.Text.Json;
using Api.Models.UserModels;
using Api.Urls.ApiUrls;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.User)]
public class UserTests
{
    [Test]
    [Order(1)]
    public async Task GetUserAsAdmin()
    {
        var url = new ApiUserUrl(TestData.AdminUserName).Relative;
        var response = await TestSetup.AuthorizedClient(TestData.AdminToken).GetAsync(url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<FullUserModel>(content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserName, Is.EqualTo(TestData.AdminUserName));
        Assert.That(result.DisplayName, Is.EqualTo(TestData.AdminDisplayName));
    }

    [Test]
    [Order(2)]
    public async Task GetUserAsUser()
    {
        var url = new ApiUserUrl(TestData.AdminUserName).Relative;
        var response = await TestSetup.AuthorizedClient(TestData.UserToken).GetAsync(url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<UserModel>(content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserName, Is.EqualTo(TestData.AdminUserName));
        Assert.That(result.DisplayName, Is.EqualTo(TestData.AdminDisplayName));
    }

    [Test]
    [Order(3)]
    public async Task ListUsersAsAdmin()
    {
        var response = await TestSetup.AuthorizedClient(TestData.AdminToken).GetAsync(new ApiUserListUrl().Relative);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<UserItemModel>>(content);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));

        Assert.That(result[0].UserName, Is.EqualTo(TestData.AdminUserName));
        Assert.That(result[0].DisplayName, Is.EqualTo(TestData.AdminDisplayName));
        Assert.That(result[1].UserName, Is.EqualTo(TestData.ManagerUserName));
        Assert.That(result[1].DisplayName, Is.EqualTo(TestData.ManagerDisplayName));
        Assert.That(result[2].UserName, Is.EqualTo(TestData.UserUserName));
        Assert.That(result[2].DisplayName, Is.EqualTo(TestData.UserDisplayName));
    }

    [Test]
    [Order(4)]
    public async Task ListUsersAsUser()
    {
        var response = await TestSetup.AuthorizedClient(TestData.UserToken).GetAsync(new ApiUserListUrl().Relative);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}