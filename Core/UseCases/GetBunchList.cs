using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetBunchList(IBunchRepository bunchRepository, IUserRepository userRepository)
    : UseCase<GetBunchList.Request, GetBunchList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await userRepository.GetByUserName(request.UserName);
        if (!AccessControl.CanListBunches(user))
            return Error(new AccessDeniedError());

        var bunches = await bunchRepository.List();
        return Success(new Result(bunches));
    }
    
    public class Request(string userName)
    {
        public string UserName { get; } = userName;
    }

    public class Result(IEnumerable<Bunch> bunches) : BunchListResult(bunches);
}

public class BunchListResult
{
    public IList<BunchListResultItem> Bunches { get; }

    public BunchListResult(IEnumerable<Bunch> bunches)
    {
        Bunches = bunches.Select(o => new BunchListResultItem(o)).ToList();
    }
}

public class BunchListResultItem(Bunch bunch)
{
    public string Slug { get; } = bunch.Slug;
    public string Name { get; } = bunch.DisplayName;
    public string Description { get; } = bunch.Description;
}