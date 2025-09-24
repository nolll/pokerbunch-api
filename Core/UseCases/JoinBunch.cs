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
    IInvitationCodeCreator invitationCodeCreator)
    : UseCase<JoinBunch.Request, JoinBunch.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var players = await playerRepository.List(request.Slug);
        var player = GetMatchedPlayer(players, request.Code);

        if (player == null)
            return Error(new InvalidJoinCodeError());
        
        await playerRepository.JoinBunch(player, bunch, request.Auth.Id);
        return Success(new Result(bunch.Slug, player.Id));
    }
    
    private Player? GetMatchedPlayer(IEnumerable<Player> players, string postedCode) => 
        players.FirstOrDefault(player => invitationCodeCreator.GetCode(player) == postedCode);

    public class Request(IAuth auth, string slug, string code)
    {
        public IAuth Auth { get; } = auth;
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