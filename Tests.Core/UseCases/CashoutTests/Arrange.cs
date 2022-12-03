using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.UseCases;
using Moq;

namespace Tests.Core.UseCases.CashoutTests;

public abstract class Arrange : UseCaseTest<Cashout>
{
    protected UseCaseResult<Cashout.Result> Result;

    private const string BunchId = "1";
    private const string CashgameId = "2";
    private const string LocationId = "3";
    private const string UserId = "4";
    protected const string PlayerId = "5";
    protected const string Slug = "slug";
    protected const string UserName = "username";
    private readonly DateTime _startTime = DateTime.Parse("2001-01-01 12:00:00");

    protected virtual int CashoutStack => 123;
    protected DateTime CashoutTime => _startTime.AddMinutes(1);
    protected virtual bool HasCashedOutBefore => false;

    protected int CheckpointCountBeforeCashout;
    protected Cashgame UpdatedCashgame;

    protected override void Setup()
    {
        var cashgame = CreateCashgame();
        var player = new Player(BunchId, PlayerId, UserId, UserName);
        var user = new User(UserId, UserName);

        CheckpointCountBeforeCashout = cashgame.Checkpoints.Count;
        UpdatedCashgame = null;

        Mock<ICashgameRepository>().Setup(s => s.Get(CashgameId)).Returns(Task.FromResult(CreateCashgame()));
        Mock<ICashgameRepository>().Setup(o => o.Update(It.IsAny<Cashgame>())).Callback((Cashgame c) => UpdatedCashgame = c);
        Mock<IPlayerRepository>().Setup(s => s.Get(BunchId, UserId)).Returns(Task.FromResult(player));
        Mock<IUserRepository>().Setup(s => s.GetByUserName(UserName)).Returns(Task.FromResult(user));
    }

    protected override async Task ExecuteAsync()
    {
        Result = await Sut.Execute(new Cashout.Request(UserName, CashgameId, PlayerId, CashoutStack, CashoutTime));
    }

    private Cashgame CreateCashgame()
    {
        if (HasCashedOutBefore)
        {
            var checkpoints1 = new List<Checkpoint>
            {
                Checkpoint.Create(CashgameId, PlayerId, _startTime, CheckpointType.Buyin, 200, 200, "1"),
                Checkpoint.Create(CashgameId, PlayerId, _startTime.AddMinutes(1), CheckpointType.Cashout, 200, 0, "3")
            };

            return new Cashgame(BunchId, LocationId, null, GameStatus.Running, CashgameId, checkpoints1);
        }
        else
        {
            var checkpoints1 = new List<Checkpoint>
            {
                Checkpoint.Create(CashgameId, PlayerId, _startTime, CheckpointType.Buyin, 200, 200, "1")
            };

            return new Cashgame(BunchId, LocationId, null, GameStatus.Running, CashgameId, checkpoints1);
        }
    }
}