using System;
using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Moq;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ReportTests
{
    public class Arrange : UseCaseTest<Report>
    {
        private string Username = "username";
        private int UserId = 1;
        private int CashgameId = 2;
        private int PlayerId = 3;
        private int BunchId = 4;
        protected virtual int Stack => 5;
        private DateTime CurrentTime = DateTime.MinValue;
        protected Cashgame UpdatedCashgame;

        protected override void Setup()
        {
            var cashgame = new CashgameInTest(bunchId: BunchId);
            var player = new PlayerInTest(id: PlayerId);
            var user = new UserInTest(id: UserId);

            Mock<ICashgameRepository>().Setup(o => o.Get(CashgameId)).Returns(cashgame);
            Mock<ICashgameRepository>().Setup(o => o.Update(It.IsAny<Cashgame>())).Callback((Cashgame cg) => UpdatedCashgame = cg);
            Mock<IPlayerRepository>().Setup(o => o.Get(BunchId, UserId)).Returns(player);
            Mock<IUserRepository>().Setup(o => o.Get(Username)).Returns(user);
        }

        protected override void Execute()
        {
            var request = new Report.Request(Username, CashgameId, PlayerId, Stack, CurrentTime);
            Sut.Execute(request);
        }
    }
}