using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetLocation(
    ILocationRepository locationRepository)
    : UseCase<GetLocation.Request, GetLocation.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var location = await locationRepository.Get(request.LocationId);
        var bunchInfo = request.Principal.GetBunchById(location.BunchId);

        return !request.Principal.CanSeeLocation(location.BunchId) 
            ? Error(new AccessDeniedError())
            : Success(new Result(location.Id, location.Name, bunchInfo.Slug));
    }

    public class Request(IPrincipal principal, string locationId)
    {
        public IPrincipal Principal { get; } = principal;
        public string LocationId { get; } = locationId;
    }

    public class Result(string id, string name, string slug)
    {
        public string Id { get; private set; } = id;
        public string Name { get; private set; } = name;
        public string Slug { get; private set; } = slug;
    }
}