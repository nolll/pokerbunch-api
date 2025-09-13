using System;
using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Moq;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ReportTests;

public class Arrange : UseCaseTest<Report>
{
    protected UseCaseResult<Report.Result>? Result;
    
    private const string CashgameId = "2";
    private const string PlayerId = "3";
    private const string BunchId = "4";
    protected virtual int Stack => 5;
    private static DateTime CurrentTime => DateTime.MinValue;
    protected Cashgame? UpdatedCashgame;

    protected override void Setup()
    {
        var cashgame = new CashgameInTest(bunchId: BunchId);

        Mock<ICashgameRepository>().Setup(o => o.Get(CashgameId)).Returns(Task.FromResult<Cashgame>(cashgame));
        Mock<ICashgameRepository>().Setup(o => o.Update(It.IsAny<Cashgame>())).Callback((Cashgame cg) => UpdatedCashgame = cg);
    }

    protected override async Task ExecuteAsync()
    {
        var request = new Report.Request(new AuthInTest(canEditCashgameActionsFor: true), CashgameId, PlayerId, Stack, CurrentTime);
        Result = await Sut.Execute(request);
    }
}