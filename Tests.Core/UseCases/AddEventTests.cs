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
        await ExecuteAsync(eventName);

        await _eventRepository.Received().Add(Arg.Is<Event>(o => o.Name == eventName));
    }

    [Fact]
    public async Task AddEvent_InvalidName_ReturnsValidationError()
    {
        var result = await ExecuteAsync("");

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    private async Task<UseCaseResult<AddEvent.Result>> ExecuteAsync(string eventName)
    {
        var userBunch = Create.UserBunch();
        var request = new AddEvent.Request(new AuthInTest(canAddEvent: true, userBunch: userBunch), Create.String(), eventName);
        return await Sut.Execute(request);
    }

    private AddEvent Sut => new(_eventRepository);
}