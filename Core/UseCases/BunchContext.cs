using Core.Entities;
using Core.Exceptions;
using Core.Repositories;

namespace Core.UseCases;

public class BunchContext : UseCase<BunchContext.Request, BunchContext.Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IBunchRepository _bunchRepository;

    public BunchContext(IUserRepository userRepository, IBunchRepository bunchRepository)
    {
        _userRepository = userRepository;
        _bunchRepository = bunchRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var appContext = new CoreContext(_userRepository).Execute(new CoreContext.Request(request.UserName));
        if (!appContext.Success)
            return Error(appContext.Error);

        var bunch = GetBunch(appContext.Data, request);
        return Success(GetResult(appContext.Data, bunch));
    }

    private Result GetResult(CoreContext.Result appContext, Bunch bunch)
    {
        return bunch != null 
            ? new Result(appContext, bunch.Slug, bunch.Id, bunch.DisplayName) 
            : new Result(appContext);
    }

    private Bunch GetBunch(CoreContext.Result appContext, Request request)
    {
        if (!appContext.IsLoggedIn)
            return null;

        if (!string.IsNullOrEmpty(request.Slug))
        {
            try
            {
                return _bunchRepository.GetBySlug(request.Slug);
            }
            catch (BunchNotFoundException)
            {
                return null;
            }
        }
        var bunches = _bunchRepository.List(appContext.UserId);
        return bunches.Count == 1 ? bunches[0] : null;
    }

    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }

        public Request(string userName, string slug = null)
        {
            UserName = userName;
            Slug = slug;
        }
    }

    public class Result
    {
        public int BunchId { get; }
        public string Slug { get; }
        public string BunchName { get; }
        public bool HasBunch { get; }
        public CoreContext.Result AppContext { get; }

        public Result(CoreContext.Result appContextResult)
        {
            AppContext = appContextResult;
        }

        public Result(CoreContext.Result appContextResult, string slug, int bunchId, string bunchName)
            : this(appContextResult)
        {
            BunchId = bunchId;
            Slug = slug;
            BunchName = bunchName;
            HasBunch = true;
        }
    }
}