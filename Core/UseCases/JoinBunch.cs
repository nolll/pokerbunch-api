using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class JoinBunch : UseCase<JoinBunch.Request, JoinBunch.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUserRepository _userRepository;

    public JoinBunch(IBunchRepository bunchRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
    {
        _bunchRepository = bunchRepository;
        _playerRepository = playerRepository;
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await _bunchRepository.GetBySlug(request.Slug);
        var players = await _playerRepository.List(bunch.Id);
        var player = GetMatchedPlayer(players, request.Code);

        if (player == null)
            return Error(new InvalidJoinCodeError());

        var user = await _userRepository.Get(request.UserName);
        await _playerRepository.JoinBunch(player, bunch, user.Id);
        return Success(new Result(bunch.Slug, player.Id));
    }
    
    private static Player GetMatchedPlayer(IEnumerable<Player> players, string postedCode)
    {
        foreach (var player in players)
        {
            var code = InvitationCodeCreator.GetCode(player);
            if (code == postedCode)
                return player;
        }
        return null;
    }

    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }
        [Required(ErrorMessage = "Code can't be empty")]
        public string Code { get; }

        public Request(string userName, string slug, string code)
        {
            UserName = userName;
            Slug = slug;
            Code = code;
        }
    }

    public class Result
    {
        public string Slug { get; private set; }
        public int PlayerId { get; private set; }

        public Result(string slug, int playerId)
        {
            Slug = slug;
            PlayerId = playerId;
        }
    }
}