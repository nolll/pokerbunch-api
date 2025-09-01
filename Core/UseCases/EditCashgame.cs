using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EditCashgame(
    ICashgameRepository cashgameRepository,
    ILocationRepository locationRepository,
    IEventRepository eventRepository)
    : UseCase<EditCashgame.Request, EditCashgame.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var cashgame = await cashgameRepository.Get(request.Id);

        if (!request.Principal.CanEditCashgame(cashgame.BunchId))
            return Error(new AccessDeniedError());

        var location = await locationRepository.Get(request.LocationId!);
        var @event = request.EventId != null ? await eventRepository.Get(request.EventId) : null;
        var eventId = @event?.Id;
        var updatedCashgame = new Cashgame(cashgame.BunchId, location.Id, eventId, cashgame.Status, cashgame.Id);
        await cashgameRepository.Update(updatedCashgame);

        if (cashgame.EventId == null && request.EventId != null)
        {
            await eventRepository.AddCashgame(request.EventId, updatedCashgame.Id);
        }
        else if(cashgame.EventId != null && request.EventId == null)
        {
            await eventRepository.RemoveCashgame(cashgame.EventId, cashgame.Id);
        }

        return Success(new Result());
    }

    public class Request(IPrincipal principal, string id, string? locationId, string? eventId)
    {
        public IPrincipal Principal { get; } = principal;
        public string Id { get; } = id;

        [Required(ErrorMessage = "Please select a location")]
        public string? LocationId { get; } = locationId;

        public string? EventId { get; } = eventId;
    }

    public class Result
    {
    }
}