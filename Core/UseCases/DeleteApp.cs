using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class DeleteApp
    {
        private readonly IAppRepository _appRepository;
        private readonly IUserRepository _userRepository;

        public DeleteApp(IAppRepository appRepository, IUserRepository userRepository)
        {
            _appRepository = appRepository;
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var app = _appRepository.Get(request.AppId);
            var user = _userRepository.Get(request.UserName);
            RequireRole.Me(user, app.UserId);

            _appRepository.Delete(request.AppId);
            return new Result();
        }

        public class Request
        {
            public string UserName { get; }
            public int AppId { get; }

            public Request(string userName, int appId)
            {
                UserName = userName;
                AppId = appId;
            }
        }

        public class Result
        {
        }
    }
}