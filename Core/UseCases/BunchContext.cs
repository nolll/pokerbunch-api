using Core.Entities;
using Core.Exceptions;
using Core.Repositories;

namespace Core.UseCases;

public class BunchContext
{
    private readonly IUserRepository _userRepository;
    private readonly IBunchRepository _bunchRepository;

    public BunchContext(IUserRepository userRepository, IBunchRepository bunchRepository)
    {
        _userRepository = userRepository;
        _bunchRepository = bunchRepository;
    }

    public Result Execute(BunchRequest request)
    {
        var appContext = new CoreContext(_userRepository).Execute(new CoreContext.Request(request.UserName));
        var bunch = GetBunch(appContext, request);
        return GetResult(appContext, bunch);
    }

    private Result GetResult(CoreContext.Result appContext, Bunch bunch)
    {
        if (bunch == null)
            return new Result(appContext);

        return new Result(appContext, bunch.Slug, bunch.Id, bunch.DisplayName);
    }

    private Bunch GetBunch(CoreContext.Result appContext, BunchRequest request)
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

    public class BunchRequest
    {
        public string UserName { get; }
        public string Slug { get; }

        public BunchRequest(string userName, string slug = null)
        {
            UserName = userName;
            Slug = slug;
        }
    }

    public class Result
    {
        public int BunchId { get; private set; }
        public string Slug { get; private set; }
        public string BunchName { get; private set; }
        public bool HasBunch { get; private set; }
        public CoreContext.Result AppContext { get; private set; }

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