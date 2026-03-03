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
        var token = await LoginHelper.GetManagerToken();
        var parameters = new AddBunchPostModel(Data.BunchDisplayName, Data.BunchDescription, Data.TimeZone, Data.CurrencySymbol, Data.CurrencyLayout);
        var result = await ApiClient.Bunch.Add(token, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model!.Name.Should().Be(Data.BunchDisplayName);
        result.Model!.Id.Should().Be(Data.BunchId);
        result.Model!.DefaultBuyin.Should().Be(0);
        result.Model!.CurrencySymbol.Should().Be(Data.CurrencySymbol);
        result.Model!.CurrencyLayout.Should().Be(Data.CurrencyLayout);
        result.Model!.CurrencyFormat.Should().Be("${0}");
        result.Model!.Description.Should().Be(Data.BunchDescription);
        result.Model!.HouseRules.Should().Be("");
        result.Model!.Timezone.Should().Be(Data.TimeZone);
        result.Model!.ThousandSeparator.Should().Be(" ");
    }

    [Fact]
    [Order(TestSuite.Bunch, 2)]
    public async Task Suite06Bunch_02ApplyForMembershipAndApprove()
    {
        var userToken = await LoginHelper.GetUserToken();
        var joinRequestResult = await ApiClient.JoinRequest.Add(userToken, Data.BunchId);
        joinRequestResult.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var managerToken = await LoginHelper.GetManagerToken();
        var joinRequestListResult = await ApiClient.JoinRequest.ListByBunch(managerToken, Data.BunchId);
        joinRequestListResult.StatusCode.Should().Be(HttpStatusCode.OK);
        var joinRequestId = joinRequestListResult.Model!.First().Id;

        EmailSender.LastSentTo.Should().Be(Data.ManagerEmail);
        EmailSender.LastMessage.Should().BeOfType<JoinRequestMessage>();

        var acceptResult = await ApiClient.JoinRequest.Accept(managerToken, joinRequestId);
        acceptResult.StatusCode.Should().Be(HttpStatusCode.OK);
        
        EmailSender.LastSentTo.Should().Be(Data.UserEmail);
        EmailSender.LastMessage.Should().BeOfType<AcceptJoinRequestMessage>();
    }

    [Fact]
    [Order(TestSuite.Bunch, 3)]
    public async Task Suite06Bunch_03AddPlayerWithoutUser()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        await AddPlayer(managerToken, Data.PlayerName);
    }

    [Fact]
    [Order(TestSuite.Bunch, 4)]
    public async Task Suite06Bunch_04GetBunchAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await ApiClient.Bunch.Get(token, Data.BunchId);
        result.Success.Should().BeFalse();
    }

    [Fact]
    [Order(TestSuite.Bunch, 5)]
    public async Task Suite06Bunch_05GetBunchAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.Bunch.Get(managerToken, Data.BunchId);
        result.Model.Should().NotBeNull();
        AssertCommonProperties(result.Model);
    }

    [Fact]
    [Order(TestSuite.Bunch, 6)]
    public async Task Suite06Bunch_06GetBunchAsUser()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await ApiClient.Bunch.Get(userToken, Data.BunchId);
        result.Model.Should().NotBeNull();
        AssertCommonProperties(result.Model);
    }

    [Fact]
    [Order(TestSuite.Bunch, 7)]
    public async Task Suite06Bunch_07UpdateBunch()
    {
        var newDescription = DataFactory.String();
        var houseRules = DataFactory.String();
        var defaultBuyin = DataFactory.Int();

        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new UpdateBunchPostModel(newDescription, houseRules, Data.TimeZone, Data.CurrencySymbol, Data.CurrencyLayout, defaultBuyin);
        var updateResult = await ApiClient.Bunch.Update(managerToken, Data.BunchId, parameters);
        updateResult.Model!.Description.Should().Be(newDescription);
        updateResult.Model!.HouseRules.Should().Be(houseRules);
        updateResult.Model!.DefaultBuyin.Should().Be(defaultBuyin);

        var getResult = await ApiClient.Bunch.Get(managerToken, Data.BunchId);
        getResult.Model!.Description.Should().Be(newDescription);
        getResult.Model!.HouseRules.Should().Be(houseRules);
        getResult.Model!.DefaultBuyin.Should().Be(defaultBuyin);
    }
    
    [Fact]
    [Order(TestSuite.Bunch, 8)]
    public async Task Suite06Bunch_08ListBunchesAsUser()
    {
        var managerToken = await LoginHelper.GetUserToken();
        var result = await ApiClient.Bunch.List(managerToken);
        result.Success.Should().BeTrue();
        var list = result.Model!.ToList();
        var first = list.First();
        list.Count.Should().Be(1);
        first.Name.Should().Be(Data.BunchDisplayName);
    }

    [Fact]
    [Order(TestSuite.Bunch, 9)]
    public async Task Suite06Bunch_09ListUserBunchesForUserWithOneBunch()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.Bunch.ListForUser(managerToken);
        result.Success.Should().BeTrue();
        var list = result.Model!.ToList();
        var first = list.First();
        list.Count.Should().Be(1);
        first.Name.Should().Be(Data.BunchDisplayName);
    }

    [Fact]
    [Order(TestSuite.Bunch, 10)]
    public async Task Suite06Bunch_10ListUserBunchesForUserWithNoBunches()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await ApiClient.Bunch.ListForUser(token);
        result.Success.Should().BeTrue();
        result.Model!.Count().Should().Be(0);
    }
    
    private async Task AddPlayer(string? token, string playerName)
    {
        var parameters = new PlayerAddPostModel(playerName);
        var result = await ApiClient.Player.Add(token, Data.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private void AssertCommonProperties(BunchModel? bunch)
    {
        bunch!.Name.Should().Be(Data.BunchDisplayName);
        bunch.Id.Should().Be(Data.BunchId);
        bunch.DefaultBuyin.Should().Be(0);
        bunch.CurrencySymbol.Should().Be(Data.CurrencySymbol);
        bunch.CurrencyLayout.Should().Be(Data.CurrencyLayout);
        bunch.CurrencyFormat.Should().Be("${0}");
        bunch.Description.Should().Be(Data.BunchDescription);
        bunch.HouseRules.Should().Be("");
        bunch.Timezone.Should().Be(Data.TimeZone);
        bunch.ThousandSeparator.Should().Be(" ");
    }
}