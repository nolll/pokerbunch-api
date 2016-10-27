using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;

namespace Core.Services
{
    public class AppService
    {
        private readonly IAppRepository _appRepository;

        public AppService(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public IList<App> ListApps()
        {
            return _appRepository.List();
        }

        public IList<App> ListApps(int userId)
        {
            return _appRepository.List(userId);
        }

        public App Get(int id)
        {
            return _appRepository.Get(id);
        }

        public App Get(string appKey)
        {
            var app = _appRepository.Get(appKey);
            if(app == null)
                throw new AppNotFoundException();
            return app;
        }

        public int Add(App app)
        {
            return _appRepository.Add(app);
        }

        public void Update(App app)
        {
            _appRepository.Update(app);
        }
    }
}