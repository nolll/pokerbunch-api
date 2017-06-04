using System;
using Core.Repositories;
using Core.UseCases;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ReportTests
{
    public class Arrange : UseCaseTest<Report>
    {
        private string Username = "";
        private int CashgameId = 0;
        private int PlayerId = 0;
        private int BunchId = 0;
        protected virtual int Stack => 0;
        private DateTime CurrentTime = DateTime.MinValue;

        protected override void Setup()
        {
            var cashgame = new CashgameInTest(bunchId: BunchId);
            var user = new UserInTest();

            Mock<ICashgameRepository>().Setup(o => o.Get(CashgameId)).Returns(cashgame);
            Mock<IPlayerRepository>();
            Mock<IUserRepository>().Setup(o => o.Get(Username)).Returns(user);
        }

        protected override void Execute()
        {
            var request = new Report.Request(Username, CashgameId, PlayerId, Stack, CurrentTime);
            Sut.Execute(request);
        }
    }
}