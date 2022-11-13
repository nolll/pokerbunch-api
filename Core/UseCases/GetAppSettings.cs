using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class RequireAppsettingsAccess
{
    private readonly IUserRepository _userRepository;

    public RequireAppsettingsAccess(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void Execute(Request request)
    {
        var user = _userRepository.Get(request.UserName);
        if (!AccessControl.CanSeeAppSettings(user))
            throw new AccessDeniedException();
    }

    public class Request
    {
        public string UserName { get; }

        public Request(string userName)
        {
            UserName = userName;
        }
    }
}