using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ClearCacheTests
{
    public abstract class Arrange : UseCaseTest<ClearCache>
    {
        protected string UserName = "user-name-1";
        protected abstract Role Role { get; }

        protected override void Setup()
        {
            var user = new UserInTest(globalRole: Role);

            Mock<IUserRepository>().Setup(o => o.Get(UserName)).Returns(user);
            Mock<ICacheContainer>().Verify(o => o.ClearAll());
        }

        protected override void Execute()
        {
        }
    }
}