using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories
{
    public interface IBunchRepository
    {
        Bunch Get(int id);
        Bunch GetBySlug(string slug);
        IList<Bunch> List(IList<int> ids);
        IList<Bunch> List();
        IList<Bunch> List(int userId);
        int Add(Bunch bunch);
        void Update(Bunch bunch);
    }
}