using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetLocation(
    ILocationRepository locationRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository,
    IBunchRepository bunchRepository)
    : UseCase<GetLocation.Request, GetLocation.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var location = await locationRepository.Get(request.LocationId);
        var bunch = await bunchRepository.Get(location.BunchId);
        var user = await userRepository.GetByUserName(request.UserName);
        var player = await playerRepository.Get(location.BunchId, user.Id);

        return !AccessControl.CanSeeLocation(user, player) 
            ? Error(new AccessDeniedError())
            : Success(new Result(location.Id, location.Name, bunch.Slug));
    }

    public class Request(string userName, string locationId)
    {
        public string UserName { get; } = userName;
        public string LocationId { get; } = locationId;
    }

    public class Result(string id, string name, string slug)
    {
        public string Id { get; private set; } = id;
        public string Name { get; private set; } = name;
        public string Slug { get; private set; } = slug;
    }
}