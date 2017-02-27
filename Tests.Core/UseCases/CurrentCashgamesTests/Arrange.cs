using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.CurrentCashgamesTests
{
    public abstract class Arrange : UseCaseTest<CurrentCashgames>
    {
        protected CurrentCashgames.Result Result;

        private const string UserName = "default-current-user";
        private const int UserId = 1;
        protected const string Slug = "default-slug";
        private const int BunchId = 2;
        protected const int CashgameId = 3;
        protected virtual Role Role => Role.Guest;
        protected virtual int GameCount => 0;

        protected override void Setup()
        {
            var user = new UserInTest(id: UserId);
            var bunch = new BunchInTest(id: BunchId, slug: Slug);
            var player = new PlayerInTest(role: Role);
            var cashgame = GameCount > 0 ? new CashgameInTest(id: CashgameId) : null;

            Mock<IUserRepository>().Setup(s => s.Get(UserName)).Returns(user);
            Mock<IBunchRepository>().Setup(s => s.GetBySlug(Slug)).Returns(bunch);
            Mock<IPlayerRepository>().Setup(s => s.Get(BunchId, UserId)).Returns(player);
            Mock<ICashgameRepository>().Setup(s => s.GetRunning(BunchId)).Returns(cashgame);
        }

        protected override void Execute()
        {
            Result = Sut.Execute(new CurrentCashgames.Request(UserName, Slug));
        }
    }
}
