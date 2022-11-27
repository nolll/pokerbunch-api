using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetLocation : UseCase<GetLocation.Request, GetLocation.Result>
{
    private readonly ILocationRepository _locationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IBunchRepository _bunchRepository;

    public GetLocation(ILocationRepository locationRepository, IUserRepository userRepository, IPlayerRepository playerRepository, IBunchRepository bunchRepository)
    {
        _locationRepository = locationRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _bunchRepository = bunchRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var location = await _locationRepository.Get(request.LocationId);
        var bunch = await _bunchRepository.Get(location.BunchId);
        var user = await _userRepository.GetByUserNameOrEmail(request.UserName);
        var player = await _playerRepository.Get(location.BunchId, user.Id);

        if (!AccessControl.CanSeeLocation(user, player))
            return Error(new AccessDeniedError());

        return Success(new Result(location.Id, location.Name, bunch.Slug));
    }

    public class Request
    {
        public string UserName { get; }
        public string LocationId { get; }

        public Request(string userName, string locationId)
        {
            UserName = userName;
            LocationId = locationId;
        }
    }

    public class Result
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }

        public Result(string id, string name, string slug)
        {
            Id = id;
            Name = name;
            Slug = slug;
        }
    }
}