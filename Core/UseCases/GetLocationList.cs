using System.Linq;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetLocationList(ILocationRepository locationRepository)
    : UseCase<GetLocationList.Request, GetLocationList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunchInfo = request.Auth.GetBunchBySlug(request.Slug);
        if (!request.Auth.CanListLocations(bunchInfo.Id))
            return Error(new AccessDeniedError());

        var locations = await locationRepository.List(bunchInfo.Id);

        var locationItems = locations.Select(o => CreateLocationItem(o, bunchInfo.Slug)).OrderBy(o => o.Name).ToList();

        return Success(new Result(locationItems));
    }

    private static Location CreateLocationItem(Entities.Location location, string slug) => 
        new(location.Id, location.Name, slug);

    public class Request(IAuth auth, string slug)
    {
        public IAuth Auth { get; } = auth;
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