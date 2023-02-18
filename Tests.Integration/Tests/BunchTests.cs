using System.Net;
using System.Text.RegularExpressions;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
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
        Assert.That(result.Model?.Name, Is.EqualTo(TestData.BunchDisplayName));
        Assert.That(result.Model?.Id, Is.EqualTo(TestData.BunchId));
        Assert.That(result.Model?.DefaultBuyin, Is.EqualTo(0));
        Assert.That(result.Model?.CurrencySymbol, Is.EqualTo(TestData.CurrencySymbol));
        Assert.That(result.Model?.CurrencyLayout, Is.EqualTo(TestData.CurrencyLayout));
        Assert.That(result.Model?.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(result.Model?.Description, Is.EqualTo(TestData.BunchDescription));
        Assert.That(result.Model?.HouseRules, Is.EqualTo(""));
        Assert.That(result.Model?.Role, Is.EqualTo("manager"));
        Assert.That(result.Model?.Timezone, Is.EqualTo(TestData.TimeZone));
        Assert.That(result.Model?.ThousandSeparator, Is.EqualTo(" "));
        Assert.That(result.Model?.Player?.Id, Is.EqualTo("1"));
        Assert.That(result.Model?.Player?.Name, Is.EqualTo(TestData.ManagerDisplayName));
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
        var inviteResult = await TestClient.Player.Invite(TestData.ManagerToken, TestData.UserPlayerId, inviteParameters);
        var lastMessageBody = TestSetup.EmailSender?.LastMessage?.Body ?? "";
        var verificationCode = GetVerificationCode(lastMessageBody);
        Assert.That(inviteResult.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(verificationCode, Is.Not.Null);

        var joinParameters = new JoinBunchPostModel(verificationCode);
        var joinResult = await TestClient.Bunch.Join(TestData.UserToken, TestData.BunchId, joinParameters);
        Assert.That(joinResult.StatusCode, Is.EqualTo(HttpStatusCode.OK));
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
        Assert.That(result.Model?.Role, Is.EqualTo("admin"));
        Assert.That(result.Model?.Player, Is.Null);
    }

    [Test]
    [Order(6)]
    public async Task GetBunchAsManager()
    {
        var result = await TestClient.Bunch.Get(TestData.ManagerToken, TestData.BunchId);
        Assert.That(result.Model, Is.Not.Null);
        AssertCommonProperties(result.Model);
        Assert.That(result.Model?.Role, Is.EqualTo("manager"));
        Assert.That(result.Model?.Player?.Id, Is.EqualTo(TestData.ManagerPlayerId));
        Assert.That(result.Model?.Player?.Name, Is.EqualTo(TestData.ManagerDisplayName));
    }

    [Test]
    [Order(7)]
    public async Task GetBunchAsUser()
    {
        var result = await TestClient.Bunch.Get(TestData.UserToken, TestData.BunchId);
        Assert.That(result.Model, Is.Not.Null);
        AssertCommonProperties(result.Model);
        Assert.That(result.Model?.Role, Is.EqualTo("player"));
        Assert.That(result.Model?.Player?.Id, Is.EqualTo(TestData.UserPlayerId));
        Assert.That(result.Model?.Player?.Name, Is.EqualTo(TestData.UserDisplayName));
    }

    [Test]
    [Order(8)]
    public async Task UpdateBunch()
    {
        var newDescription = $"UPDATED: {TestData.BunchDescription}";
        var houseRules = "UPDATED: house rules";
        var defaultBuyin = 10_000;

        var parameters = new UpdateBunchPostModel(newDescription, houseRules, TestData.TimeZone, TestData.CurrencySymbol, TestData.CurrencyLayout, defaultBuyin);
        var updateResult = await TestClient.Bunch.Update(TestData.ManagerToken, TestData.BunchId, parameters);
        Assert.That(updateResult.Model?.Description, Is.EqualTo(newDescription));
        Assert.That(updateResult.Model?.HouseRules, Is.EqualTo(houseRules));
        Assert.That(updateResult.Model?.DefaultBuyin, Is.EqualTo(defaultBuyin));

        var getResult = await TestClient.Bunch.Get(TestData.ManagerToken, TestData.BunchId);
        Assert.That(getResult.Model?.Description, Is.EqualTo(newDescription));
        Assert.That(getResult.Model?.HouseRules, Is.EqualTo(houseRules));
        Assert.That(getResult.Model?.DefaultBuyin, Is.EqualTo(defaultBuyin));
    }

    [Test]
    [Order(9)]
    public async Task ListBunchesAsAdmin()
    {
        var result = await TestClient.Bunch.List(TestData.AdminToken);
        Assert.That(result.Success, Is.True);
        var list = result.Model?.ToList();
        var first = list?.First();
        Assert.That(list?.Count, Is.EqualTo(1));
        Assert.That(first?.Name, Is.EqualTo(TestData.BunchDisplayName));
    }

    [Test]
    [Order(10)]
    public async Task ListBunhesAsManager()
    {
        var result = await TestClient.Bunch.List(TestData.ManagerToken);
        Assert.That(result.Success, Is.False);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [Order(11)]
    public async Task ListUserBunchesForUserWithOneBunch()
    {
        var result = await TestClient.Bunch.ListForUser(TestData.ManagerToken);
        Assert.That(result.Success, Is.True);
        var list = result.Model?.ToList();
        var first = list?.First();
        Assert.That(list?.Count, Is.EqualTo(1));
        Assert.That(first?.Name, Is.EqualTo(TestData.BunchDisplayName));
    }

    [Test]
    [Order(12)]
    public async Task ListUserBunchesForUserWithNoBunches()
    {
        var result = await TestClient.Bunch.ListForUser(TestData.AdminToken);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Model?.Count(), Is.EqualTo(0));
    }

    private string GetVerificationCode(string messageBody)
    {
        var regex = new Regex("verification code: (.+)");
        return regex.Match(messageBody).Groups[1].Value.Trim();
    }

    private async Task AddPlayer(string? token, string playerName)
    {
        var parameters = new PlayerAddPostModel(playerName);
        var result = await TestClient.Player.Add(token, TestData.BunchId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private void AssertCommonProperties(BunchModel? bunch)
    {
        Assert.That(bunch?.Name, Is.EqualTo(TestData.BunchDisplayName));
        Assert.That(bunch?.Id, Is.EqualTo(TestData.BunchId));
        Assert.That(bunch?.DefaultBuyin, Is.EqualTo(0));
        Assert.That(bunch?.CurrencySymbol, Is.EqualTo(TestData.CurrencySymbol));
        Assert.That(bunch?.CurrencyLayout, Is.EqualTo(TestData.CurrencyLayout));
        Assert.That(bunch?.CurrencyFormat, Is.EqualTo("${0}"));
        Assert.That(bunch?.Description, Is.EqualTo(TestData.BunchDescription));
        Assert.That(bunch?.HouseRules, Is.EqualTo(""));
        Assert.That(bunch?.Timezone, Is.EqualTo(TestData.TimeZone));
        Assert.That(bunch?.ThousandSeparator, Is.EqualTo(" "));
    }
}