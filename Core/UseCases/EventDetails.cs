using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EventDetails(
    IEventRepository eventRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository,
    IBunchRepository bunchRepository,
    ILocationRepository locationRepository)
    : UseCase<EventDetails.Request, EventDetails.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var e = await eventRepository.Get(request.EventId);
        var location = e.LocationId != null ? await locationRepository.Get(e.LocationId) : null;
        var bunch = await bunchRepository.Get(e.BunchId);
        var user = await userRepository.GetByUserName(request.UserName);
        var player = await playerRepository.Get(e.BunchId, user.Id);

        if (!AccessControl.CanSeeEventDetails(user, player))
            return Error(new AccessDeniedError());

        var locationId = location?.Id;
        var locationName = location?.Name;

        return Success(new Result(e.Id, e.Name, bunch.Slug, locationId, locationName, e.StartDate));
    }

    public class Request(string userName, string eventId)
    {
        public string UserName { get; } = userName;
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