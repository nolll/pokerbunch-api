using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DeleteCashgame : UseCase<DeleteCashgame.Request, DeleteCashgame.Result>
{
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IBunchRepository _bunchRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;

    public DeleteCashgame(ICashgameRepository cashgameRepository, IBunchRepository bunchRepository, IUserRepository userRepository, IPlayerRepository playerRepository)
    {
        _cashgameRepository = cashgameRepository;
        _bunchRepository = bunchRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var cashgame = await _cashgameRepository.Get(request.Id);
        var bunch = await _bunchRepository.Get(cashgame.BunchId);
        var currentUser = await _userRepository.GetByUserNameOrEmail(request.UserName);
        var currentPlayer = await _playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanDeleteCashgame(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        if (cashgame.EventId != null)
            return Error(new CashgameIsPartOfEventError());

        if (cashgame.PlayerCount > 0)
            return Error(new CashgameHasResultsError());

        await _cashgameRepository.DeleteGame(cashgame.Id);

        return Success(new Result(bunch.Slug));
    }
    
    public class Request
    {
        public string UserName { get; }
        public string Id { get; }

        public Request(string userName, string id)
        {
            UserName = userName;
            Id = id;
        }
    }

    public class Result
    {
        public string Slug { get; private set; }

        public Result(string slug)
        {
            Slug = slug;
        }
    }
}