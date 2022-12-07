using Core.Entities;
using Core.Repositories;

namespace Tests.Common.FakeRepositories;

public class FakeUserRepository : IUserRepository
{
    public User Added { get; private set; }
    public User Saved { get; private set; }
    private readonly IList<User> _list;

    public FakeUserRepository()
    {
        _list = CreateList();
    }

    public Task<User> GetById(string id)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.Id == id));
    }

    public Task<IList<User>> List()
    {
        return Task.FromResult(_list);
    }

    public Task<User> GetByUserName(string userName)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.UserName == userName));
    }

    public Task<User> GetByUserEmail(string email)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.Email == email));
    }

    public Task<User> GetByUserNameOrEmail(string userNameOrEmail)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.UserName == userNameOrEmail || o.Email == userNameOrEmail));
    }

    public Task<string> Add(User user)
    {
        Added = user;
        return Task.FromResult("1");
    }

    public Task Update(User user)
    {
        Saved = user;
        return Task.CompletedTask;
    }

    private IList<User> CreateList()
    {
        return new List<User>
        {
            TestData.UserA,
            TestData.UserB,
            TestData.UserC,
            TestData.UserD
        };
    }
}