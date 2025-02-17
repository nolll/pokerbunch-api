using System.Linq;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetLocationList(
    IBunchRepository bunchRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository,
    ILocationRepository locationRepository)
    : UseCase<GetLocationList.Request, GetLocationList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var user = await userRepository.GetByUserName(request.UserName);
        var player = await playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanListLocations(user, player))
            return Error(new AccessDeniedError());

        var locations = await locationRepository.List(bunch.Id);

        var locationItems = locations.Select(o => CreateLocationItem(o, bunch.Slug)).OrderBy(o => o.Name).ToList();

        return Success(new Result(locationItems));
    }

    private static Location CreateLocationItem(Entities.Location location, string slug) => 
        new(location.Id, location.Name, slug);

    public class Request(string userName, string slug)
    {
        public string UserName { get; } = userName;
        public string Slug { get; } = slug;
    }

    public class Result(IList<Location> locations)
    {
        public IList<Location> Locations { get; } = locations;
    }

    public class Location(string id, string name, string slug)
    {
        public string Id { get; } = id;
        public string Name { get; } = name;
        public string Slug { get; } = slug;
    }
}