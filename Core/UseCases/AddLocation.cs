using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddLocation : AsyncUseCase<AddLocation.Request, AddLocation.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILocationRepository _locationRepository;

    public AddLocation(IBunchRepository bunchRepository, IPlayerRepository playerRepository, IUserRepository userRepository, ILocationRepository locationRepository)
    {
        _bunchRepository = bunchRepository;
        _playerRepository = playerRepository;
        _userRepository = userRepository;
        _locationRepository = locationRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = _bunchRepository.GetBySlug(request.Slug);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanAddLocation(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var location = new Location(0, request.Name, bunch.Id);
        var id = await _locationRepository.Add(location);

        return Success(new Result(bunch.Slug, id, location.Name));
    }
    
    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }
        [Required(ErrorMessage = "Name can't be empty")]
        public string Name { get; }

        public Request(string userName, string slug, string name)
        {
            UserName = userName;
            Slug = slug;
            Name = name;
        }
    }

    public class Result
    {
        public string Slug { get; }
        public int Id { get; }
        public string Name { get; }

        public Result(string slug, int id, string name)
        {
            Slug = slug;
            Id = id;
            Name = name;
        }
    }
}