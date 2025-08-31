using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EventDetails(
    IEventRepository eventRepository,
    ILocationRepository locationRepository)
    : UseCase<EventDetails.Request, EventDetails.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var e = await eventRepository.Get(request.EventId);
        var location = e.LocationId != null ? await locationRepository.Get(e.LocationId) : null;
        var bunchInfo = request.Principal.GetBunchById(e.BunchId);

        if (!request.Principal.CanSeeEventDetails(e.BunchId))
            return Error(new AccessDeniedError());

        var locationId = location?.Id;
        var locationName = location?.Name;

        return Success(new Result(e.Id, e.Name, bunchInfo.Slug, locationId, locationName, e.StartDate));
    }

    public class Request(IPrincipal principal, string eventId)
    {
        public IPrincipal Principal { get; } = principal;
        public string EventId { get; } = eventId;
    }

    public class Result(
        string id,
        string name,
        string bunchId,
        string? locationId,
        string? locationName,
        Date startDate)
    {
        public string Id { get; } = id;
        public string Name { get; } = name;
        public string BunchId { get; } = bunchId;
        public string? LocationId { get; } = locationId;
        public string? LocationName { get; } = locationName;
        public Date StartDate { get; } = startDate;
    }
}