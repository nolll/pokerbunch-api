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
[Order(TestOrder.Bunch)]
public class BunchTests
{
    [Test]
    [Order(1)]
    public async Task CreateBunch()
    {
        var parameters = new AddBunchPostModel(TestData.BunchDisplayName, TestData.BunchDescription, TestData.TimeZone, TestData.CurrencySymbol, TestData.CurrencyLayout);
        var result = await TestClient.Bunch.Add(TestData.ManagerToken, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.Name, Is.EqualTo(TestData.BunchDisplayName));
        Assert.That(result.Model.Id, Is.EqualTo(TestData.BunchId));
        Assert.That(result.Model.DefaultBuyin, Is.EqualTo(0));
        Assert.That(result.Model.CurrencySymbol, Is.EqualTo(TestData.CurrencySymbol));
        Assert.That(result.Model.CurrencyLayout, Is.EqualTo(TestData.CurrencyLayout));
        Assert.That(result.Model.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(result.Model.Description, Is.EqualTo(TestData.BunchDescription));
        Assert.That(result.Model.HouseRules, Is.EqualTo(""));
        Assert.That(result.Model.Role, Is.EqualTo("manager"));
        Assert.That(result.Model.Timezone, Is.EqualTo(TestData.TimeZone));
        Assert.That(result.Model.ThousandSeparator, Is.EqualTo(" "));
        Assert.That(result.Model.Player.Id, Is.EqualTo("1"));
        Assert.That(result.Model.Player.Name, Is.EqualTo(TestData.ManagerDisplayName));
    }

    [Test]
    [Order(2)]
    public async Task AddPlayerForUser()
    {
        await AddPlayer(TestData.ManagerToken, TestData.UserPlayerName);
    }

    [Test]
    [Order(3)]
    public async Task InviteAndJoin()
    {
        var inviteParameters = new PlayerInvitePostModel(TestData.UserEmail);
        var inviteUrl = new ApiPlayerInviteUrl(TestData.UserPlayerIdString).Relative;
        await TestClient.Post(TestData.ManagerToken, inviteUrl, inviteParameters);
        var verificationCode = GetVerificationCode(TestSetup.EmailSender.LastMessage);
        Assert.That(verificationCode, Is.Not.Null);

        var joinParameters = new JoinBunchPostModel(verificationCode);
        var joinUrl = new ApiBunchJoinUrl(TestData.BunchId).Relative;
        var response = await TestClient.Post(TestData.UserToken, joinUrl, joinParameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(4)]
    public async Task AddPlayerWithoutUser()
    {
        await AddPlayer(TestData.ManagerToken, TestData.PlayerName);
    }

    [Test]
    [Order(5)]
    public async Task GetBunchAsAdmin()
    {
        var result = await TestClient.Bunch.Get(TestData.AdminToken, TestData.BunchId);
        Assert.That(result.Model, Is.Not.Null);
        AssertCommonProperties(result.Model);
        Assert.That(result.Model.Role, Is.EqualTo("admin"));
        Assert.That(result.Model.Player, Is.Null);
    }

    [Test]
    [Order(6)]
    public async Task GetBunchAsManager()
    {
        var result = await TestClient.Bunch.Get(TestData.ManagerToken, TestData.BunchId);
        Assert.That(result.Model, Is.Not.Null);
        AssertCommonProperties(result.Model);
        Assert.That(result.Model.Role, Is.EqualTo("manager"));
        Assert.That(result.Model.Player.Id, Is.EqualTo(TestData.ManagerPlayerIdString));
        Assert.That(result.Model.Player.Name, Is.EqualTo(TestData.ManagerDisplayName));
    }

    [Test]
    [Order(7)]
    public async Task GetBunchAsUser()
    {
        var result = await TestClient.Bunch.Get(TestData.UserToken, TestData.BunchId);
        Assert.That(result.Model, Is.Not.Null);
        AssertCommonProperties(result.Model);
        Assert.That(result.Model.Role, Is.EqualTo("player"));
        Assert.That(result.Model.Player.Id, Is.EqualTo(TestData.UserPlayerIdString));
        Assert.That(result.Model.Player.Name, Is.EqualTo(TestData.UserDisplayName));
    }

    private string GetVerificationCode(IMessage message)
    {
        var regex = new Regex("verification code: (.+)");
        return regex.Match(message.Body).Groups[1].Value.Trim();
    }

    private async Task AddPlayer(string token, string playerName)
    {
        var parameters = new PlayerAddPostModel(playerName);
        var result = await TestClient.Player.Add(token, TestData.BunchId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
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