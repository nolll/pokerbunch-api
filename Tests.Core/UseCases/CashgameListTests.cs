using System.Collections.Generic;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class CashgameListTests : TestBase
{
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();

    [Fact]
    public async Task CashgameList_WithoutGames_HasEmptyListOfGames()
    {
        var bunch = Create.Bunch();
        _cashgameRepository.GetFinished(bunch.Id).Returns([]);

        var request = CreateRequest(bunch.Id, bunch.Slug);
        var result = await Sut.Execute(request);
        
        result.Data!.Items.Count.Should().Be(0);
    }

    [Fact]
    public async Task CashgameList_ReturnsList()
    {
        var bunch = Create.Bunch();
        var location = Create.Location(bunchId: bunch.Id);
        var cashgame = Create.Cashgame(bunchId: bunch.Id, locationId: location.Id);
        _cashgameRepository.GetFinished(bunch.Id, Arg.Any<int?>()).Returns([cashgame]);
        _playerRepository.Get(Arg.Any<IList<string>>()).Returns([]);
        _locationRepository.List(bunch.Id).Returns([location]);

        var request = CreateRequest(bunch.Id, bunch.Slug);
        var result = await Sut.Execute(request);

        result.Success.Should().BeTrue();
        result.Data!.Slug.Should().Be(bunch.Slug);
        result.Data!.Items[0].LocationName.Should().Be(location.Name);
        result.Data!.Items[0].CashgameId.Should().Be(cashgame.Id);
    }
    
    private CashgameList.Request CreateRequest(string bunchId, string slug)
    {
        var userBunch = Create.UserBunch(bunchId, slug);
        return new CashgameList.Request(new AuthInTest(canListCashgames: true, userBunch: userBunch), slug, null);
    }

    private CashgameList Sut => new(
        _cashgameRepository,
        _playerRepository,
        _locationRepository);
}