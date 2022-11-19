using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class RequireAppsettingsAccess : AsyncUseCase<RequireAppsettingsAccess.Request, RequireAppsettingsAccess.Result>
{
    private readonly IUserRepository _userRepository;

    public RequireAppsettingsAccess(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await _userRepository.Get(request.UserName);
        if (!AccessControl.CanSeeAppSettings(user))
            return Error(new AccessDeniedError());

        return Success(new Result());
    }
    
    public class Request
    {
        public string UserName { get; }

        public Request(string userName)
        {
            UserName = userName;
        }
    }

    public class Result
    {
    }
}