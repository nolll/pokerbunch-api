using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Api.Urls.ApiUrls;
using Core;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.BunchAndPlayer)]
public class BunchAndPlayerTests
{
    [Test]
    [Order(1)]
    public async Task CreateBunch()
    {
        var parameters = new AddBunchPostModel(TestData.BunchDisplayName, TestData.BunchDescription, TestData.TimeZone, TestData.CurrencySymbol, TestData.CurrencyLayout);
        var response = await TestSetup.AuthorizedClient(TestData.ManagerToken).PostAsJsonAsync(new ApiBunchAddUrl().Relative, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(TestData.BunchDisplayName));
        Assert.That(result.Id, Is.EqualTo(TestData.BunchId));
        Assert.That(result.DefaultBuyin, Is.EqualTo(0));
        Assert.That(result.CurrencySymbol, Is.EqualTo(TestData.CurrencySymbol));
        Assert.That(result.CurrencyLayout, Is.EqualTo(TestData.CurrencyLayout));
        Assert.That(result.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(result.Description, Is.EqualTo(TestData.BunchDescription));
        Assert.That(result.HouseRules, Is.EqualTo(""));
        Assert.That(result.Role, Is.EqualTo("manager"));
        Assert.That(result.Timezone, Is.EqualTo(TestData.TimeZone));
        Assert.That(result.ThousandSeparator, Is.EqualTo(" "));
        Assert.That(result.Player.Id, Is.EqualTo("1"));
        Assert.That(result.Player.Name, Is.EqualTo(TestData.ManagerDisplayName));
    }

    [Test]
    [Order(2)]
    public async Task AddPlayerForUser()
    {
        await AddPlayer(TestData.ManagerToken, TestData.UserPlayerName, TestData.UserPlayerIdString);
    }

    [Test]
    [Order(3)]
    public async Task InviteAndJoin()
    {
        var inviteParameters = new PlayerInvitePostModel(TestData.UserEmail);
        var inviteUrl = new ApiPlayerInviteUrl(TestData.UserPlayerIdString).Relative;
        await TestSetup.AuthorizedClient(TestData.ManagerToken).PostAsJsonAsync(inviteUrl, inviteParameters);
        var verificationCode = GetVerificationCode(TestSetup.EmailSender.LastMessage);
        Assert.That(verificationCode, Is.Not.Null);

        var joinParameters = new JoinBunchPostModel(verificationCode);
        var joinUrl = new ApiBunchJoinUrl(TestData.BunchId).Relative;
        var response = await TestSetup.AuthorizedClient(TestData.UserToken).PostAsJsonAsync(joinUrl, joinParameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(4)]
    public async Task AddPlayerWithoutUser()
    {
        await AddPlayer(TestData.ManagerToken, TestData.PlayerName, TestData.PlayerPlayerIdString);
    }

    [Test]
    [Order(5)]
    public async Task GetBunchAsAdmin()
    {
        var url = new ApiBunchUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(TestData.AdminToken).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonProperties(result);
        Assert.That(result.Role, Is.EqualTo("admin"));
        Assert.That(result.Player, Is.Null);
    }

    [Test]
    [Order(6)]
    public async Task GetBunchAsManager()
    {
        var url = new ApiBunchUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(TestData.ManagerToken).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonProperties(result);
        Assert.That(result.Role, Is.EqualTo("manager"));
        Assert.That(result.Player.Id, Is.EqualTo(TestData.ManagerPlayerIdString));
        Assert.That(result.Player.Name, Is.EqualTo(TestData.ManagerDisplayName));
    }

    [Test]
    [Order(7)]
    public async Task GetBunchAsUser()
    {
        var url = new ApiBunchUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(TestData.UserToken).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<BunchModel>(content);
        Assert.That(result, Is.Not.Null);
        AssertCommonProperties(result);
        Assert.That(result.Role, Is.EqualTo("player"));
        Assert.That(result.Player.Id, Is.EqualTo(TestData.UserPlayerIdString));
        Assert.That(result.Player.Name, Is.EqualTo(TestData.UserDisplayName));
    }

    private string GetVerificationCode(IMessage message)
    {
        var regex = new Regex("verification code: (.+)");
        return regex.Match(message.Body).Groups[1].Value.Trim();
    }

    private async Task AddPlayer(string token, string playerName, string expectedId)
    {
        var parameters = new PlayerAddPostModel(playerName);
        var url = new ApiPlayerAddUrl(TestData.BunchId).Relative;
        var response = await TestSetup.AuthorizedClient(token).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PlayerModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(playerName));
        Assert.That(result.Id, Is.EqualTo(expectedId));
        Assert.That(result.Slug, Is.EqualTo(TestData.BunchId));
        Assert.That(result.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(result.AvatarUrl, Is.EqualTo(""));
        Assert.That(result.UserId, Is.EqualTo(""));
        Assert.That(result.UserName, Is.EqualTo(""));
    }

    private void AssertCommonProperties(BunchModel bunch)
    {
        Assert.That(bunch.Name, Is.EqualTo(TestData.BunchDisplayName));
        Assert.That(bunch.Id, Is.EqualTo(TestData.BunchId));
        Assert.That(bunch.DefaultBuyin, Is.EqualTo(0));
        Assert.That(bunch.CurrencySymbol, Is.EqualTo(TestData.CurrencySymbol));
        Assert.That(bunch.CurrencyLayout, Is.EqualTo(TestData.CurrencyLayout));
        Assert.That(bunch.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(bunch.Description, Is.EqualTo(TestData.BunchDescription));
        Assert.That(bunch.HouseRules, Is.EqualTo(""));
        Assert.That(bunch.Timezone, Is.EqualTo(TestData.TimeZone));
        Assert.That(bunch.ThousandSeparator, Is.EqualTo(" "));
    }
}