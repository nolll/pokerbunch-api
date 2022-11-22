using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetBunchList : UseCase<GetBunchList.Request, GetBunchList.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IUserRepository _userRepository;

    public GetBunchList(IBunchRepository bunchRepository, IUserRepository userRepository)
    {
        _bunchRepository = bunchRepository;
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await _userRepository.GetByUserNameOrEmail(request.UserName);
        if (!AccessControl.CanListBunches(user))
            return Error(new AccessDeniedError());

        var bunches = await _bunchRepository.List();
        return Success(new Result(bunches));
    }
    
    public class Request
    {
        public string UserName { get; }

        public Request(string userName)
        {
            UserName = userName;
        }
    }

    public class Result : BunchListResult
    {
        public Result(IEnumerable<Bunch> bunches) : base(bunches)
        {
        }
    }
}

public class BunchListResult
{
    public IList<BunchListResultItem> Bunches { get; }

    public BunchListResult(IEnumerable<Bunch> bunches)
    {
        Bunches = bunches.Select(o => new BunchListResultItem(o)).ToList();
    }
}

public class BunchListResultItem
{
    public string Slug { get; }
    public string Name { get; }
    public string Description { get; }

    public BunchListResultItem(Bunch bunch)
    {
        Slug = bunch.Slug;
        Name = bunch.DisplayName;
        Description = bunch.Description;
    }
}