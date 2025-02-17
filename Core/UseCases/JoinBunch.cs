using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class JoinBunch(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository,
    IUserRepository userRepository,
    IInvitationCodeCreator invitationCodeCreator)
    : UseCase<JoinBunch.Request, JoinBunch.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var players = await playerRepository.List(bunch.Id);
        var player = GetMatchedPlayer(players, request.Code);

        if (player == null)
            return Error(new InvalidJoinCodeError());

        var user = await userRepository.GetByUserName(request.UserName);
        await playerRepository.JoinBunch(player, bunch, user.Id);
        return Success(new Result(bunch.Slug, player.Id));
    }
    
    private Player? GetMatchedPlayer(IEnumerable<Player> players, string postedCode) => 
        players.FirstOrDefault(player => invitationCodeCreator.GetCode(player) == postedCode);

    public class Request(string userName, string slug, string code)
    {
        public string UserName { get; } = userName;
        public string Slug { get; } = slug;

        [Required(ErrorMessage = "Code can't be empty")]
        public string Code { get; } = code;
    }

    public class Result(string slug, string playerId)
    {
        public string Slug { get; private set; } = slug;
        public string PlayerId { get; private set; } = playerId;
    }
}