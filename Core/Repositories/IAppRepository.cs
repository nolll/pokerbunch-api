using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories
{
    public interface IAppRepository
    {
        App Get(int id);
        App Get(string appKey);
        IList<App> List();
        IList<App> List(int userId);
        int Add(App app);
        void Update(App app);
    }
}
