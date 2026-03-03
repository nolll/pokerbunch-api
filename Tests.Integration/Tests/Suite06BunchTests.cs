using System.Net;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Core.Messages;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Bunch, 1)]
    public async Task Suite06Bunch_01CreateBunch()
    {
        var token = await fixture.LoginHelper.GetManagerToken();
        var parameters = new AddBunchPostModel(TestData.BunchDisplayName, TestData.BunchDescription, TestData.TimeZone, TestData.CurrencySymbol, TestData.CurrencyLayout);
        var result = await fixture.ApiClient.Bunch.Add(token, parameters);
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

    [Fact]
    [Order(TestSuite.Bunch, 2)]
    public async Task Suite06Bunch_02ApplyForMembershipAndApprove()
    {
        var userToken = await fixture.LoginHelper.GetUserToken();
        var joinRequestResult = await fixture.ApiClient.JoinRequest.Add(userToken, TestData.BunchId);
        joinRequestResult.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var joinRequestListResult = await fixture.ApiClient.JoinRequest.ListByBunch(managerToken, TestData.BunchId);
        joinRequestListResult.StatusCode.Should().Be(HttpStatusCode.OK);
        var joinRequestId = joinRequestListResult.Model!.First().Id;

        fixture.EmailSender!.LastSentTo.Should().Be(TestData.ManagerEmail);
        fixture.EmailSender.LastMessage.Should().BeOfType<JoinRequestMessage>();

        var acceptResult = await fixture.ApiClient.JoinRequest.Accept(managerToken, joinRequestId);
        acceptResult.StatusCode.Should().Be(HttpStatusCode.OK);
        
        fixture.EmailSender!.LastSentTo.Should().Be(TestData.UserEmail);
        fixture.EmailSender.LastMessage.Should().BeOfType<AcceptJoinRequestMessage>();
    }

    [Fact]
    [Order(TestSuite.Bunch, 3)]
    public async Task Suite06Bunch_03AddPlayerWithoutUser()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        await AddPlayer(managerToken, TestData.PlayerName);
    }

    [Fact]
    [Order(TestSuite.Bunch, 4)]
    public async Task Suite06Bunch_04GetBunchAsAdmin()
    {
        var token = await fixture.LoginHelper.GetAdminToken();
        var result = await fixture.ApiClient.Bunch.Get(token, TestData.BunchId);
        result.Success.Should().BeFalse();
    }

    [Fact]
    [Order(TestSuite.Bunch, 5)]
    public async Task Suite06Bunch_05GetBunchAsManager()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.Bunch.Get(managerToken, TestData.BunchId);
        result.Model.Should().NotBeNull();
        AssertCommonProperties(result.Model);
    }

    [Fact]
    [Order(TestSuite.Bunch, 6)]
    public async Task Suite06Bunch_06GetBunchAsUser()
    {
        var userToken = await fixture.LoginHelper.GetUserToken();
        var result = await fixture.ApiClient.Bunch.Get(userToken, TestData.BunchId);
        result.Model.Should().NotBeNull();
        AssertCommonProperties(result.Model);
    }

    [Fact]
    [Order(TestSuite.Bunch, 7)]
    public async Task Suite06Bunch_07UpdateBunch()
    {
        const string newDescription = $"UPDATED: {TestData.BunchDescription}";
        const string houseRules = "UPDATED: house rules";
        const int defaultBuyin = 10_000;

        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var parameters = new UpdateBunchPostModel(newDescription, houseRules, TestData.TimeZone, TestData.CurrencySymbol, TestData.CurrencyLayout, defaultBuyin);
        var updateResult = await fixture.ApiClient.Bunch.Update(managerToken, TestData.BunchId, parameters);
        updateResult.Model!.Description.Should().Be(newDescription);
        updateResult.Model!.HouseRules.Should().Be(houseRules);
        updateResult.Model!.DefaultBuyin.Should().Be(defaultBuyin);

        var getResult = await fixture.ApiClient.Bunch.Get(managerToken, TestData.BunchId);
        getResult.Model!.Description.Should().Be(newDescription);
        getResult.Model!.HouseRules.Should().Be(houseRules);
        getResult.Model!.DefaultBuyin.Should().Be(defaultBuyin);
    }
    
    [Fact]
    [Order(TestSuite.Bunch, 8)]
    public async Task Suite06Bunch_08ListBunchesAsUser()
    {
        var managerToken = await fixture.LoginHelper.GetUserToken();
        var result = await fixture.ApiClient.Bunch.List(managerToken);
        result.Success.Should().BeTrue();
        var list = result.Model!.ToList();
        var first = list.First();
        list.Count.Should().Be(1);
        first.Name.Should().Be(TestData.BunchDisplayName);
    }

    [Fact]
    [Order(TestSuite.Bunch, 9)]
    public async Task Suite06Bunch_09ListUserBunchesForUserWithOneBunch()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.Bunch.ListForUser(managerToken);
        result.Success.Should().BeTrue();
        var list = result.Model!.ToList();
        var first = list.First();
        list.Count.Should().Be(1);
        first.Name.Should().Be(TestData.BunchDisplayName);
    }

    [Fact]
    [Order(TestSuite.Bunch, 10)]
    public async Task Suite06Bunch_10ListUserBunchesForUserWithNoBunches()
    {
        var token = await fixture.LoginHelper.GetAdminToken();
        var result = await fixture.ApiClient.Bunch.ListForUser(token);
        result.Success.Should().BeTrue();
        result.Model!.Count().Should().Be(0);
    }
    
    private async Task AddPlayer(string? token, string playerName)
    {
        var parameters = new PlayerAddPostModel(playerName);
        var result = await fixture.ApiClient.Player.Add(token, TestData.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private static void AssertCommonProperties(BunchModel? bunch)
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