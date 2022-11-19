using System.Linq;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetLocationList : UseCase<GetLocationList.Request, GetLocationList.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILocationRepository _locationRepository;

    public GetLocationList(IBunchRepository bunchRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository)
    {
        _bunchRepository = bunchRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _locationRepository = locationRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await _bunchRepository.GetBySlug(request.Slug);
        var user = await _userRepository.Get(request.UserName);
        var player = await _playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanListLocations(user, player))
            return Error(new AccessDeniedError());

        var locations = await _locationRepository.List(bunch.Id);

        var locationItems = locations.Select(o => CreateLocationItem(o, bunch.Slug)).OrderBy(o => o.Name).ToList();

        return Success(new Result(locationItems));
    }

    private static Location CreateLocationItem(Entities.Location location, string slug)
    {
        return new Location(location.Id, location.Name, slug);
    }

    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }

        public Request(string userName, string slug)
        {
            UserName = userName;
            Slug = slug;
        }
    }

    public class Result
    {
        public IList<Location> Locations { get; }

        public Result(IList<Location> locations)
        {
            Locations = locations;
        }
    }

    public class Location
    {
        public int Id { get; }
        public string Name { get; }
        public string Slug { get; }

        public Location(int id, string name, string slug)
        {
            Id = id;
            Name = name;
            Slug = slug;
        }
    }
}