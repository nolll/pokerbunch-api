using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EnsureAdmin
{
    private readonly IUserRepository _userRepository;

    public EnsureAdmin(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void Execute(Request request)
    {
        var user = _userRepository.Get(request.UserName);
        RequireRole.Admin(user);
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