using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ClearCacheTests;

public abstract class Arrange : UseCaseTest<ClearCache>
{
    protected UseCaseResult<ClearCache.Result> Result;

    protected string UserName = "user-name-1";
    protected abstract Role Role { get; }

    protected override void Setup()
    {
        var user = new UserInTest(globalRole: Role);

        Mock<IUserRepository>().Setup(o => o.Get(UserName)).Returns(user);
    }

    protected override void Execute()
    {
        Result = Sut.Execute(new ClearCache.Request(UserName));
    }
}