using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EventDetails : UseCase<EventDetails.Request, EventDetails.Result>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IBunchRepository _bunchRepository;
    private readonly ILocationRepository _locationRepository;

    public EventDetails(IEventRepository eventRepository, IUserRepository userRepository, IPlayerRepository playerRepository, IBunchRepository bunchRepository, ILocationRepository locationRepository)
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _bunchRepository = bunchRepository;
        _locationRepository = locationRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var e = await _eventRepository.Get(request.EventId);
        var location = e.LocationId != null ? await _locationRepository.Get(e.LocationId) : null;
        var bunch = await _bunchRepository.Get(e.BunchId);
        var user = await _userRepository.GetByUserName(request.UserName);
        var player = await _playerRepository.Get(e.BunchId, user.Id);

        if (!AccessControl.CanSeeEventDetails(user, player))
            return Error(new AccessDeniedError());

        var locationId = location?.Id;
        var locationName = location?.Name;

        return Success(new Result(e.Id, e.Name, bunch.Slug, locationId, locationName, e.StartDate));
    }

    public class Request
    {
        public string UserName { get; }
        public string EventId { get; }

        public Request(string userName, string eventId)
        {
            UserName = userName;
            EventId = eventId;
        }
    }

    public class Result
    {
        public string Id { get; }
        public string Name { get; }
        public string BunchId { get; }
        public string LocationId { get; }
        public string LocationName { get; }
        public Date StartDate { get; }

        public Result(string id, string name, string bunchId, string locationId, string locationName, Date startDate)
        {
            Id = id;
            Name = name;
            BunchId = bunchId;
            LocationId = locationId;
            LocationName = locationName;
            StartDate = startDate;
        }
    }
}