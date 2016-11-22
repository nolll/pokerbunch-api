using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class GetApp
    {
        private readonly IAppRepository _appRepository;
        private readonly IUserRepository _userRepository;

        public GetApp(IAppRepository appRepository, IUserRepository userRepository)
        {
            _appRepository = appRepository;
            _userRepository = userRepository;
        }

        public AppResult Execute(Request request)
        {
            var app = _appRepository.Get(request.AppId);
            var user = _userRepository.Get(request.CurrentUserName);
            RequireRole.Me(user, app.UserId);

            return new AppResult(app.Id, app.AppKey, app.Name);
        }

        public class Request
        {
            public string CurrentUserName { get; }
            public int AppId { get; }

            public Request(string currentUserName, int appId)
            {
                CurrentUserName = currentUserName;
                AppId = appId;
            }
        }
    }

    public class AppResult
    {
        public int AppId { get; private set; }
        public string AppKey { get; private set; }
        public string AppName { get; private set; }

        public AppResult(int appId, string appKey, string appName)
        {
            AppId = appId;
            AppKey = appKey;
            AppName = appName;
        }
    }
}