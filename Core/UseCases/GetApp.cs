using Core.Repositories;

namespace Core.UseCases
{
    public class GetApp
    {
        private readonly IAppRepository _appRepository;
        
        public GetApp(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public AppResult Execute(Request request)
        {
            var app = _appRepository.Get(request.AppId);

            return new AppResult(app.Id, app.AppKey, app.Name);
        }

        public class Request
        {
            public int AppId { get; }

            public Request(int appId)
            {
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