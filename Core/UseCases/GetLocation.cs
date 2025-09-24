using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetLocation(ILocationRepository locationRepository)
    : UseCase<GetLocation.Request, GetLocation.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var location = await locationRepository.Get(request.LocationId);

        return !request.Auth.CanSeeLocation(location.BunchSlug) 
            ? Error(new AccessDeniedError())
            : Success(new Result(location.Id, location.Name, location.BunchSlug));
    }

    public class Request(IAuth auth, string locationId)
    {
        public IAuth Auth { get; } = auth;
        public string LocationId { get; } = locationId;
    }

    public class Result(string id, string name, string slug)
    {
        public string Id { get; private set; } = id;
        public string Name { get; private set; } = name;
        public string Slug { get; private set; } = slug;
    }
}