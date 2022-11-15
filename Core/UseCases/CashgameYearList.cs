using System.Collections.Generic;
using System.Linq;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class CashgameYearList : UseCase<CashgameYearList.Request, CashgameYearList.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;

    public CashgameYearList(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository)
    {
        _bunchRepository = bunchRepository;
        _cashgameRepository = cashgameRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var bunch = _bunchRepository.GetBySlug(request.Slug);
        var user = _userRepository.Get(request.UserName);
        var player = _playerRepository.Get(bunch.Id, user.Id);
        RequireRole.Player(user, player);
        var years = _cashgameRepository.GetYears(bunch.Id);

        return Success(new Result(request.Slug, years.ToList()));
    }
    
    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }

        public Request(string userName, string slug)
        {
            UserName = userName;
            Slug = slug;
        }
    }

    public class Result
    {
        public IList<int> Years { get; }
        public string Slug { get; }

        public Result(string slug, IList<int> years)
        {
            Slug = slug;
            Years = years;
        }
    }
}