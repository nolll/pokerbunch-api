using System;
using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Moq;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ReportTests;

public class Arrange : UseCaseTest<Report>
{
    protected UseCaseResult<Report.Result> Result;

    private const string Username = "username";
    private const int UserId = 1;
    private const int CashgameId = 2;
    private const int PlayerId = 3;
    private const int BunchId = 4;
    protected virtual int Stack => 5;
    private static DateTime CurrentTime => DateTime.MinValue;
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
        Result = Sut.Execute(request);
    }
}