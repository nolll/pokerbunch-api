using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class EditCashgameTests : TestBase
{
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();
    private readonly IEventRepository _eventRepository = Substitute.For<IEventRepository>();

    [Fact]
    public async Task EditCashgame_NoAccess_ReturnsError()
    {
        var cashgame = Create.Cashgame();
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);
        
        var request = CreateRequest(cashgameId: cashgame.Id, canEditCashgame: false);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task EditCashgame_EmptyLocation_ReturnsError()
    {
        var request = CreateRequest(locationId: "");
        
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task EditCashgame_ValidLocation_SavesCashgame()
    {
        var cashgame = Create.Cashgame();
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);
        var location = Create.Location();
        _locationRepository.Get(location.Id).Returns(location);
        
        var request = CreateRequest(cashgameId: cashgame.Id, locationId: location.Id);
        var result = await Sut.Execute(request);

        await _cashgameRepository.Received().Update(Arg.Is<Cashgame>(o => o.Id == cashgame.Id && o.LocationId == location.Id));
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task EditCashgame_WithEventId_GameIsAddedToEvent()
    {
        var @event = Create.Event();
        _eventRepository.Get(@event.Id).Returns(@event);
        var cashgame = Create.Cashgame();
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);
        var location = Create.Location();
        _locationRepository.Get(location.Id).Returns(location);
        
        var request = CreateRequest(cashgameId: cashgame.Id, locationId: location.Id, eventId: @event.Id);
        await Sut.Execute(request);

        await _eventRepository.Received().AddCashgame(@event.Id, cashgame.Id);
    }
    
    [Fact]
    public async Task EditCashgame_WithoutEventId_GameIsRemovedFromEvent()
    {
        var @event = Create.Event();
        _eventRepository.Get(@event.Id).Returns(@event);
        var cashgame = Create.Cashgame(eventId: @event.Id);
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);
        var location = Create.Location();
        _locationRepository.Get(location.Id).Returns(location);
        
        var request = CreateRequest(cashgameId: cashgame.Id, locationId: location.Id, eventId: null);
        await Sut.Execute(request);

        await _eventRepository.Received().RemoveCashgame(@event.Id, cashgame.Id);
    }

    private EditCashgame.Request CreateRequest(
        string? cashgameId = null, 
        string? locationId = null,
        string? eventId = null,
        bool? canEditCashgame = null)
    {
        return new EditCashgame.Request(
            new AuthInTest(canEditCashgame: canEditCashgame ?? true), 
            cashgameId ?? Create.String(), 
            locationId ?? Create.String(), 
            eventId);
    }

    private EditCashgame Sut => new(
        _cashgameRepository,
        _locationRepository,
        _eventRepository);
}