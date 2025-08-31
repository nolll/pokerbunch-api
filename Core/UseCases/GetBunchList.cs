using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetBunchList(IBunchRepository bunchRepository)
    : UseCase<GetBunchList.Request, GetBunchList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        if (!request.Principal.CanListBunches)
            return Error(new AccessDeniedError());

        var bunches = await bunchRepository.List();
        return Success(new Result(bunches));
    }
    
    public class Request(IPrincipal principal)
    {
        public IPrincipal Principal { get; } = principal;
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