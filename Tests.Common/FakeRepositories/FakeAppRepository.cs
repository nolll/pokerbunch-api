using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;

namespace Tests.Common.FakeRepositories
{
    public class FakeAppRepository : IAppRepository
    {
        private readonly IList<App> _list;
        public App Added { get; private set; }

        public FakeAppRepository()
        {
            _list = CreateList();
        }

        public App Get(int id)
        {
            return _list.FirstOrDefault(o => o.Id == id);
        }

        public IList<App> List()
        {
            return _list;
        }

        public IList<App> List(int userId)
        {
            throw new System.NotImplementedException();
        }

        public App Get(string appKey)
        {
            var app = _list.FirstOrDefault(o => o.AppKey == appKey);
            if(app == null)
                throw new AppNotFoundException();
            return app;
        }

        public int Add(App app)
        {
            Added = app;
            const int id = 1000;
            _list.Add(new App(id, app.AppKey, app.Name, app.UserId));
            return id;
        }

        public void Update(App app)
        {
            throw new System.NotImplementedException();
        }

        private IList<App> CreateList()
        {
            return new List<App>
            {
                TestData.AppA,
                TestData.AppB
            };
        }
    }
}