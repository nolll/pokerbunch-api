using System.Net;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Core.Messages;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Login, 0)]
    public async Task Suite06Bunch_01CreateBunch()
    {
        var user = await Fixture.CreateUser();
        
        var parameters = new AddBunchPostModel(
            "Bunch 1",
            DataFactory.String(), 
            "Europe/Stockholm", 
            "$", 
            "{SYMBOL}{AMOUNT}");
        var bunchResult = await ApiClient.Bunch.Add(user.Token, parameters);
        var bunch = bunchResult.Model;
        
        bunch.Should().NotBeNull();
        bunch.Name.Should().Be(parameters.Name);
        bunch.Id.Should().Be("bunch-1");
        bunch.DefaultBuyin.Should().Be(0);
        bunch.CurrencySymbol.Should().Be(parameters.CurrencySymbol);
        bunch.CurrencyLayout.Should().Be(parameters.CurrencyLayout);
        bunch.CurrencyFormat.Should().Be(parameters.CurrencyLayout
            .Replace("{SYMBOL}", parameters.CurrencySymbol)
            .Replace("{AMOUNT}", "{0}"));
        bunch.Description.Should().Be(parameters.Description);
        bunch.HouseRules.Should().Be("");
        bunch.Timezone.Should().Be(parameters.Timezone);
        bunch.ThousandSeparator.Should().Be(" ");
    }

    [Fact]
    [Order(TestSuite.Bunch, 2)]
    public async Task Suite06Bunch_02ApplyForMembershipAndApprove()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await Fixture.CreateUser();
        
        var joinRequestResult = await ApiClient.JoinRequest.Add(player.Token, bunch.Id);
        joinRequestResult.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var joinRequestListResult = await ApiClient.JoinRequest.ListByBunch(manager.Token, bunch.Id);
        joinRequestListResult.StatusCode.Should().Be(HttpStatusCode.OK);
        var joinRequestId = joinRequestListResult.Model!.First().Id;

        EmailSender.LastSentTo.Should().Be(manager.Email);
        EmailSender.LastMessage.Should().BeOfType<JoinRequestMessage>();

        var acceptResult = await ApiClient.JoinRequest.Accept(manager.Token, joinRequestId);
        acceptResult.StatusCode.Should().Be(HttpStatusCode.OK);
        
        EmailSender.LastSentTo.Should().Be(player.Email);
        EmailSender.LastMessage.Should().BeOfType<AcceptJoinRequestMessage>();
    }

    [Fact]
    [Order(TestSuite.Bunch, 3)]
    public async Task Suite06Bunch_03AddPlayerWithoutUser()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        
        var parameters = new PlayerAddPostModel(DataFactory.String());
        var result = await ApiClient.Player.Add(manager.Token, bunch.Id, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.Bunch, 4)]
    public async Task Suite06Bunch_04GetBunchAsAdmin()
    {
        var admin = await Fixture.CreateUser();
        await admin.AsAdmin();
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        
        var result = await ApiClient.Bunch.Get(admin.Token, bunch.Id);
        result.Success.Should().BeFalse();
    }

    [Fact]
    [Order(TestSuite.Bunch, 5)]
    public async Task Suite06Bunch_05GetBunchAsManager()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var result = await ApiClient.Bunch.Get(manager.Token, bunch.Id);
        result.Model.Should().NotBeNull();
        AssertCommonProperties(result.Model, bunch);
    }

    [Fact]
    [Order(TestSuite.Bunch, 6)]
    public async Task Suite06Bunch_06GetBunchAsPlayer()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await Fixture.CreateUser();
        
        await bunch.AddPlayer(player);
        
        var result = await ApiClient.Bunch.Get(player.Token, bunch.Id);
        result.Model.Should().NotBeNull();
        AssertCommonProperties(result.Model, bunch);
    }

    [Fact]
    [Order(TestSuite.Bunch, 7)]
    public async Task Suite06Bunch_07UpdateBunch()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        
        var parameters = new UpdateBunchPostModel(
            DataFactory.String(), 
            DataFactory.String(), 
            "America/Jamaica", 
            "kr", 
            "{AMOUNT} {SYMBOL}", 
            DataFactory.Int());
        
        var updateResult = await ApiClient.Bunch.Update(manager.Token, bunch.Id, parameters);
        
        updateResult.Model!.Description.Should().Be(parameters.Description);
        updateResult.Model!.HouseRules.Should().Be(parameters.HouseRules);
        updateResult.Model!.Timezone.Should().Be(parameters.Timezone);
        updateResult.Model!.CurrencySymbol.Should().Be(parameters.CurrencySymbol);
        updateResult.Model!.CurrencyLayout.Should().Be(parameters.CurrencyLayout);
        updateResult.Model!.CurrencyFormat.Should().Be("{0} kr");
        updateResult.Model!.DefaultBuyin.Should().Be(parameters.DefaultBuyin);
    }
    
    [Fact]
    [Order(TestSuite.Bunch, 8)]
    public async Task Suite06Bunch_08ListBunches()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var otherUser = await Fixture.CreateUser();
        
        var result = await ApiClient.Bunch.List(otherUser.Token);
        result.Success.Should().BeTrue();
        var list = result.Model!.ToList();
        var first = list.First();
        list.Count.Should().Be(1);
        first.Name.Should().Be(bunch.Name);
    }

    [Fact]
    [Order(TestSuite.Bunch, 9)]
    public async Task Suite06Bunch_09ListUserBunchesForUserWithOneBunch()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        
        var result = await ApiClient.Bunch.ListForUser(manager.Token);
        result.Success.Should().BeTrue();
        var list = result.Model!.ToList();
        var first = list.First();
        list.Count.Should().Be(1);
        first.Name.Should().Be(bunch.Name);
    }

    [Fact]
    [Order(TestSuite.Bunch, 10)]
    public async Task Suite06Bunch_10ListUserBunchesForUserWithNoBunches()
    {
        var manager = await Fixture.CreateUser();
        await Fixture.CreateBunch(manager);
        var player = await Fixture.CreateUser();
        var result = await ApiClient.Bunch.ListForUser(player.Token);
        result.Success.Should().BeTrue();
        result.Model!.Count().Should().Be(0);
    }

    private static void AssertCommonProperties(BunchModel? bunch, BunchFixture bunchFixture)
    {
        bunch!.Name.Should().Be(bunchFixture.Name);
        bunch.Id.Should().Be(bunchFixture.Id);
        bunch.DefaultBuyin.Should().Be(0);
        bunch.CurrencySymbol.Should().Be(bunchFixture.CurrencySymbol);
        bunch.CurrencyLayout.Should().Be(bunchFixture.CurrencyLayout);
        bunch.CurrencyFormat.Should().Be("${0}");
        bunch.Description.Should().Be(bunchFixture.Description);
        bunch.HouseRules.Should().Be("");
        bunch.Timezone.Should().Be(bunchFixture.Timezone);
        bunch.ThousandSeparator.Should().Be(" ");
    }
}