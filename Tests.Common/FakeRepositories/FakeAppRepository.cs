using System.Collections.Generic;
using System.Linq;
using Core.Entities;
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

        public IList<App> GetList(IList<int> ids)
        {
            return _list;
        }

        public IList<int> Find()
        {
            return _list.Select(o => o.Id).ToList();
        }

        public IList<int> Find(int userId)
        {
            throw new System.NotImplementedException();
        }

        public IList<int> Find(string appKey)
        {
            return _list.Where(o => o.AppKey == appKey).Select(o => o.Id).ToList();
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