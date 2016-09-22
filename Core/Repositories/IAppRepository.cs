using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories
{
    public interface IAppRepository
    {
        App Get(int id);
        IList<App> GetList(IList<int> ids);
        IList<int> Find();
        IList<int> Find(int userId);
        IList<int> Find(string appKey);
        int Add(App app);
        void Update(App app);
    }
}
