using System.Net;
using System.Text.RegularExpressions;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;

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
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model!.Name.Should().Be(TestData.BunchDisplayName);
        result.Model!.Id.Should().Be(TestData.BunchId);
        result.Model!.DefaultBuyin.Should().Be(0);
        result.Model!.CurrencySymbol.Should().Be(TestData.CurrencySymbol);
        result.Model!.CurrencyLayout.Should().Be(TestData.CurrencyLayout);
        result.Model!.CurrencyFormat.Should().Be("${0}");
        result.Model!.Description.Should().Be(TestData.BunchDescription);
        result.Model!.HouseRules.Should().Be("");
        result.Model!.Timezone.Should().Be(TestData.TimeZone);
        result.Model!.ThousandSeparator.Should().Be(" ");
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
        var invitationUrl1 = GetRelativeInvitationUrl1(lastMessageBody);
        var invitationUrl2 = GetRelativeInvitationUrl2(lastMessageBody);
        inviteResult.StatusCode.Should().Be(HttpStatusCode.OK);
        verificationCode.Should().NotBeNull();
        invitationUrl1.Should().Be($"/bunches/bunch-1/join/{verificationCode}");
        invitationUrl2.Should().Be("/bunches/bunch-1/join");

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
        result.Success.Should().BeFalse();
    }

    [Test]
    [Order(6)]
    public async Task Test06GetBunchAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Bunch.Get(managerToken, TestData.BunchId);
        result.Model.Should().NotBeNull();
        AssertCommonProperties(result.Model);
    }

    [Test]
    [Order(7)]
    public async Task Test07GetBunchAsUser()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await TestClient.Bunch.Get(userToken, TestData.BunchId);
        result.Model.Should().NotBeNull();
        AssertCommonProperties(result.Model);
    }

    [Test]
    [Order(8)]
    public async Task Test08UpdateBunch()
    {
        const string newDescription = $"UPDATED: {TestData.BunchDescription}";
        const string houseRules = "UPDATED: house rules";
        const int defaultBuyin = 10_000;

        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new UpdateBunchPostModel(newDescription, houseRules, TestData.TimeZone, TestData.CurrencySymbol, TestData.CurrencyLayout, defaultBuyin);
        var updateResult = await TestClient.Bunch.Update(managerToken, TestData.BunchId, parameters);
        updateResult.Model!.Description.Should().Be(newDescription);
        updateResult.Model!.HouseRules.Should().Be(houseRules);
        updateResult.Model!.DefaultBuyin.Should().Be(defaultBuyin);

        var getResult = await TestClient.Bunch.Get(managerToken, TestData.BunchId);
        getResult.Model!.Description.Should().Be(newDescription);
        getResult.Model!.HouseRules.Should().Be(houseRules);
        getResult.Model!.DefaultBuyin.Should().Be(defaultBuyin);
    }
    
    [Test]
    [Order(9)]
    public async Task Test10ListBunchesAsUser()
    {
        var managerToken = await LoginHelper.GetUserToken();
        var result = await TestClient.Bunch.List(managerToken);
        result.Success.Should().BeTrue();
        var list = result.Model!.ToList();
        var first = list.First();
        list.Count.Should().Be(1);
        first.Name.Should().Be(TestData.BunchDisplayName);
    }

    [Test]
    [Order(10)]
    public async Task Test11ListUserBunchesForUserWithOneBunch()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Bunch.ListForUser(managerToken);
        result.Success.Should().BeTrue();
        var list = result.Model!.ToList();
        var first = list.First();
        list.Count.Should().Be(1);
        first.Name.Should().Be(TestData.BunchDisplayName);
    }

    [Test]
    [Order(11)]
    public async Task Test12ListUserBunchesForUserWithNoBunches()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await TestClient.Bunch.ListForUser(token);
        result.Success.Should().BeTrue();
        result.Model!.Count().Should().Be(0);
    }

    private static string GetVerificationCode(string messageBody) => 
        GetMatch(messageBody, new("verification code: (.+)"));

    private static string GetRelativeInvitationUrl1(string messageBody) => 
        GetRelativeInvitationUrl(messageBody, new("invitation: (.+)\\."));

    private static string GetRelativeInvitationUrl2(string messageBody) => 
        GetRelativeInvitationUrl(messageBody, new("instead, (.+),"));

    private static string GetRelativeInvitationUrl(string messageBody, Regex regex) => 
        new Uri(regex.Match(messageBody).Groups[1].Value.Trim()).AbsolutePath;

    private static string GetMatch(string messageBody, Regex regex) => regex.Match(messageBody).Groups[1].Value.Trim();

    private async Task AddPlayer(string? token, string playerName)
    {
        var parameters = new PlayerAddPostModel(playerName);
        var result = await TestClient.Player.Add(token, TestData.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private void AssertCommonProperties(BunchModel? bunch)
    {
        bunch!.Name.Should().Be(TestData.BunchDisplayName);
        bunch.Id.Should().Be(TestData.BunchId);
        bunch.DefaultBuyin.Should().Be(0);
        bunch.CurrencySymbol.Should().Be(TestData.CurrencySymbol);
        bunch.CurrencyLayout.Should().Be(TestData.CurrencyLayout);
        bunch.CurrencyFormat.Should().Be("${0}");
        bunch.Description.Should().Be(TestData.BunchDescription);
        bunch.HouseRules.Should().Be("");
        bunch.Timezone.Should().Be(TestData.TimeZone);
        bunch.ThousandSeparator.Should().Be(" ");
    }
}