using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Moq;
using NUnit.Framework;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.CurrentCashgamesTests
{
    public abstract class Arrange
    {
        protected CurrentCashgames Sut;

        private const string UserName = "default-current-user";
        private const int UserId = 1;
        protected const string Slug = "default-slug";
        private const int BunchId = 2;
        protected const int CashgameId = 3;
        protected virtual Role Role => Role.Guest;
        protected virtual int GameCount => 0;

        [SetUp]
        public void Setup()
        {
            var urm = new Mock<IUserRepository>();
            urm.Setup(s => s.Get(UserName)).Returns(new UserInTest(id: UserId));

            var bsm = new Mock<IBunchRepository>();
            bsm.Setup(s => s.GetBySlug(Slug)).Returns(new BunchInTest(id: BunchId, slug: Slug));

            var crm = new Mock<ICashgameRepository>();
            if(GameCount > 0)
                crm.Setup(s => s.GetRunning(BunchId)).Returns(new CashgameInTest(id: CashgameId));

            var prm = new Mock<IPlayerRepository>();
            prm.Setup(s => s.Get(BunchId, UserId)).Returns(new PlayerInTest(role: Role));
            
            Sut = new CurrentCashgames(urm.Object, bsm.Object, crm.Object, prm.Object);
        }

        protected CurrentCashgames.Request Request => new CurrentCashgames.Request(UserName, Slug);
    }
}
