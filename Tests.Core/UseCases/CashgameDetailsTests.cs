using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class CashgameDetailsTests : TestBase
{
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();
    private readonly IEventRepository _eventRepository = Substitute.For<IEventRepository>();

    [Fact]
    public async Task CashgameDetails_NoAccess_ReturnsError()
    {
        var cashgame = Create.Cashgame();
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);

        var request = CreateRequest(cashgameId: cashgame.Id, canSeeCashgames: false);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task CashgameDetails_CashgameRunning_AllSimplePropertiesAreSet()
    {
        var bunch = Create.Bunch();
        _bunchRepository.GetBySlug(bunch.Slug).Returns(bunch);
        var location = Create.Location(bunchSlug: bunch.Slug);
        _locationRepository.Get(location.Id).Returns(location);
        var cashgame = Create.Cashgame(locationId: location.Id, bunchSlug: bunch.Slug, bunchId: bunch.Id, status: GameStatus.Running);
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);
        var player1 = Create.Player();
        var player2 = Create.Player();
        _playerRepository.Get(Arg.Any<IList<string>>()).Returns([player1, player2]);
        var action1 = Create.BuyinAction(cashgameId: cashgame.Id, playerId: player1.Id);
        var action2 = Create.BuyinAction(cashgameId: cashgame.Id, playerId: player2.Id);
        cashgame.SetCheckpoints([action1, action2]);

        var request = CreateRequest(bunch.Id, bunch.Slug, bunch.DisplayName, player1.Id, cashgame.Id);
        var result = await Sut.Execute(request);

        result.Success.Should().BeTrue();
        result.Data!.Slug.Should().Be(bunch.Slug);
        result.Data!.PlayerId.Should().Be(player1.Id);
        result.Data!.LocationName.Should().Be(location.Name);
        result.Data!.DefaultBuyin.Should().Be(bunch.DefaultBuyin);
        result.Data!.Role.Should().Be(Role.Player);
        
        result.Data!.PlayerItems.Count.Should().Be(2);
        var p1 = result.Data!.PlayerItems.First(o => o.PlayerId == player1.Id);
        var p2 = result.Data!.PlayerItems.First(o => o.PlayerId == player2.Id);
        
        p1.Checkpoints.Count.Should().Be(1);
        p1.HasCashedOut.Should().BeFalse();
        p1.Name.Should().Be(player1.DisplayName);
        p1.PlayerId.Should().Be(player1.Id);
        p1.CashgameId.Should().Be(cashgame.Id);
        p2.Checkpoints.Count.Should().Be(1);
        p2.HasCashedOut.Should().BeFalse();
        p2.Name.Should().Be(player2.DisplayName);
        p2.PlayerId.Should().Be(player2.Id);
        p2.CashgameId.Should().Be(cashgame.Id);
    }

    private CashgameDetails.Request CreateRequest(
        string? bunchId = null,
        string? slug = null,
        string? bunchName = null,
        string? playerId = null,
        string? cashgameId = null,
        bool? canSeeCashgames = null)
    {
        var userBunch = Create.UserBunch(
            bunchId, 
            slug, 
            bunchName,
            playerId,
            role: Role.Player);
        return new CashgameDetails.Request(
            new AuthInTest(canSeeCashgame: canSeeCashgames ?? true, userBunch: userBunch), 
            cashgameId ?? Create.String(), 
            DateTime.UtcNow);
    }
    
    private CashgameDetails Sut => new(
        _bunchRepository,
        _cashgameRepository,
        _playerRepository,
        _locationRepository,
        _eventRepository);
}