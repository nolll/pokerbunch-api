using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.UseCases;
using Moq;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.CashoutTests;

public abstract class Arrange : UseCaseTest<Cashout>
{
    protected UseCaseResult<Cashout.Result>? Result;

    private const string BunchId = "1";
    private const string CashgameId = "2";
    private const string LocationId = "3";
    protected const string PlayerId = "5";
    private readonly DateTime _startTime = DateTime.Parse("2001-01-01 12:00:00");

    protected virtual int CashoutStack => 123;
    protected DateTime CashoutTime => _startTime.AddMinutes(1);
    protected virtual bool HasCashedOutBefore => false;

    protected int CheckpointCountBeforeCashout;
    protected Cashgame? UpdatedCashgame;

    protected override void Setup()
    {
        var cashgame = CreateCashgame();

        CheckpointCountBeforeCashout = cashgame.Checkpoints.Count;
        UpdatedCashgame = null;

        Mock<ICashgameRepository>().Setup(s => s.Get(CashgameId)).Returns(Task.FromResult(CreateCashgame()));
        Mock<ICashgameRepository>().Setup(o => o.Update(It.IsAny<Cashgame>())).Callback((Cashgame c) => UpdatedCashgame = c);
    }

    protected override async Task ExecuteAsync()
    {
        Result = await Sut.Execute(new Cashout.Request(new AuthInTest(canEditCashgameActionsFor: true), CashgameId, PlayerId, CashoutStack, CashoutTime));
    }

    private Cashgame CreateCashgame()
    {
        if (HasCashedOutBefore)
        {
            var checkpoints1 = new List<Checkpoint>
            {
                Checkpoint.Create("1", CashgameId, PlayerId, _startTime, CheckpointType.Buyin, 200, 200),
                Checkpoint.Create("3", CashgameId, PlayerId, _startTime.AddMinutes(1), CheckpointType.Cashout, 200, 0)
            };

            return new Cashgame(BunchId, LocationId, null, GameStatus.Running, CashgameId, checkpoints1);
        }
        else
        {
            var checkpoints1 = new List<Checkpoint>
            {
                Checkpoint.Create("1", CashgameId, PlayerId, _startTime, CheckpointType.Buyin, 200, 200)
            };

            return new Cashgame(BunchId, LocationId, null, GameStatus.Running, CashgameId, checkpoints1);
        }
    }
}