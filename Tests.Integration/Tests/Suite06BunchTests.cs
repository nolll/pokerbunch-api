using System.Net;
using System.Text.RegularExpressions;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Core;
using FluentAssertions;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Bunch)]
public class Suite06BunchTests
{
    [Test]
    [Order(1)]
    public async Task Test01CreateBunch()
    {
        var token = await LoginHelper.GetManagerToken();
        var parameters = new AddBunchPostModel(TestData.BunchDisplayName, TestData.BunchDescription, TestData.TimeZone, TestData.CurrencySymbol, TestData.CurrencyLayout);
        var result = await TestClient.Bunch.Add(token, parameters);
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
        Assert.That(result.Model?.Timezone, Is.EqualTo(TestData.TimeZone));
        Assert.That(result.Model?.ThousandSeparator, Is.EqualTo(" "));
    }

    [Test]
    [Order(2)]
    public async Task Test02AddPlayerForUser()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        await AddPlayer(managerToken, TestData.UserPlayerName);
    }

    [Test]
    [Order(3)]
    public async Task Test03InviteAndJoin()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var inviteParameters = new PlayerInvitePostModel(TestData.UserEmail);
        var inviteResult = await TestClient.Player.Invite(managerToken, TestData.UserPlayerId, inviteParameters);
        var lastMessageBody = TestSetup.EmailSender?.LastMessage?.Body ?? "";
        var verificationCode = GetVerificationCode(lastMessageBody);
        var invitationUrl1 = GetInvitationUrl1(lastMessageBody);
        var invitationUrl2 = GetInvitationUrl2(lastMessageBody);
        inviteResult.StatusCode.Should().Be(HttpStatusCode.OK);
        verificationCode.Should().NotBeNull();
        invitationUrl1.Should().Be($"https://localhost:9001/bunches/bunch-1/join/{verificationCode}");
        invitationUrl2.Should().Be("https://localhost:9001/bunches/bunch-1/join");

        var userToken = await LoginHelper.GetUserToken();
        var joinParameters = new JoinBunchPostModel(verificationCode);
        var joinResult = await TestClient.Bunch.Join(userToken, TestData.BunchId, joinParameters);
        joinResult.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    [Order(4)]
    public async Task Test04AddPlayerWithoutUser()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        await AddPlayer(managerToken, TestData.PlayerName);
    }

    [Test]
    [Order(5)]
    public async Task Test05GetBunchAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await TestClient.Bunch.Get(token, TestData.BunchId);
        Assert.That(result.Success, Is.False);
    }

    [Test]
    [Order(6)]
    public async Task Test06GetBunchAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Bunch.Get(managerToken, TestData.BunchId);
        Assert.That(result.Model, Is.Not.Null);
        AssertCommonProperties(result.Model);
    }

    [Test]
    [Order(7)]
    public async Task Test07GetBunchAsUser()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await TestClient.Bunch.Get(userToken, TestData.BunchId);
        Assert.That(result.Model, Is.Not.Null);
        AssertCommonProperties(result.Model);
    }

    [Test]
    [Order(8)]
    public async Task Test08UpdateBunch()
    {
        var newDescription = $"UPDATED: {TestData.BunchDescription}";
        var houseRules = "UPDATED: house rules";
        var defaultBuyin = 10_000;

        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new UpdateBunchPostModel(newDescription, houseRules, TestData.TimeZone, TestData.CurrencySymbol, TestData.CurrencyLayout, defaultBuyin);
        var updateResult = await TestClient.Bunch.Update(managerToken, TestData.BunchId, parameters);
        Assert.That(updateResult.Model?.Description, Is.EqualTo(newDescription));
        Assert.That(updateResult.Model?.HouseRules, Is.EqualTo(houseRules));
        Assert.That(updateResult.Model?.DefaultBuyin, Is.EqualTo(defaultBuyin));

        var getResult = await TestClient.Bunch.Get(managerToken, TestData.BunchId);
        Assert.That(getResult.Model?.Description, Is.EqualTo(newDescription));
        Assert.That(getResult.Model?.HouseRules, Is.EqualTo(houseRules));
        Assert.That(getResult.Model?.DefaultBuyin, Is.EqualTo(defaultBuyin));
    }

    [Test]
    [Order(9)]
    public async Task Test09ListBunchesAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await TestClient.Bunch.List(token);
        Assert.That(result.Success, Is.True);
        var list = result.Model?.ToList();
        var first = list?.First();
        Assert.That(list?.Count, Is.EqualTo(1));
        Assert.That(first?.Name, Is.EqualTo(TestData.BunchDisplayName));
    }

    [Test]
    [Order(10)]
    public async Task Test10ListBunchesAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Bunch.List(managerToken);
        Assert.That(result.Success, Is.False);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [Order(11)]
    public async Task Test11ListUserBunchesForUserWithOneBunch()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Bunch.ListForUser(managerToken);
        Assert.That(result.Success, Is.True);
        var list = result.Model?.ToList();
        var first = list?.First();
        Assert.That(list?.Count, Is.EqualTo(1));
        Assert.That(first?.Name, Is.EqualTo(TestData.BunchDisplayName));
    }

    [Test]
    [Order(12)]
    public async Task Test12ListUserBunchesForUserWithNoBunches()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await TestClient.Bunch.ListForUser(token);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Model?.Count(), Is.EqualTo(0));
    }

    private static string GetVerificationCode(string messageBody)
    {
        var regex = new Regex("verification code: (.+)");
        return regex.Match(messageBody).Groups[1].Value.Trim();
    }
    
    private static string GetInvitationUrl1(string messageBody)
    {
        var regex = new Regex("invitation: (.+)\\.");
        return regex.Match(messageBody).Groups[1].Value.Trim();
    }
    
    private static string GetInvitationUrl2(string messageBody)
    {
        var regex = new Regex("instead, (.+),");
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