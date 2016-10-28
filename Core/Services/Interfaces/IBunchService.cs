using System.Collections.Generic;
using Core.Entities;

namespace Core.Services
{
    public interface IBunchService
    {
        Bunch Get(int id);
        Bunch GetBySlug(string slug);
        IList<Bunch> List(int userId);
        IList<Bunch> List();
        int Add(Bunch bunch);
        void Save(Bunch bunch);
    }
}