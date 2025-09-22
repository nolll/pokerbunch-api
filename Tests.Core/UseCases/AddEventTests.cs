using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class AddEventTests : TestBase
{
    private readonly IEventRepository _eventRepository = Substitute.For<IEventRepository>();
    
    [Fact]
    public async Task AddEvent_AllOk_EventIsAdded()
    {
        var eventName = Create.String();
        
        var request = CreateRequest(eventName);
        await Sut.Execute(request);

        await _eventRepository.Received().Add(Arg.Is<Event>(o => o.Name == eventName));
    }

    [Fact]
    public async Task AddEvent_InvalidName_ReturnsValidationError()
    {
        var request = CreateRequest("");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    private AddEvent.Request CreateRequest(string eventName)
    {
        var userBunch = Create.UserBunch();
        return new AddEvent.Request(new AuthInTest(canAddEvent: true, userBunch: userBunch), Create.String(), eventName);
    }

    private AddEvent Sut => new(_eventRepository);
}