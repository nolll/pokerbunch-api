using System;
using System.Collections.Generic;
using AutoFixture;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class CashoutTests : TestBase
{
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();
    
    [Fact]
    public async Task InvalidStack_ReturnsError()
    {
        var result = await ExecuteAsync(stack: -1);
        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task HasCashedOutBefore_UpdatesAction()
    {
        var cashoutStack = Create.Int();
        var startTime = DateTime.Parse("2001-01-01 12:00:00");
        var playerId = Create.String();
        var cashgame = Create.Cashgame();
        cashgame.SetCheckpoints([
            Create.BuyinAction("1", cashgame.Id, playerId, startTime, 200, 200),
            Create.CashoutAction("3", cashgame.Id, playerId, startTime.AddMinutes(1), 200)
        ]);
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);

        await ExecuteAsync(cashgame.Id, playerId, cashoutStack, startTime.AddMinutes(2));
        
        await _cashgameRepository.Received().Update(Arg.Is<Cashgame>(o =>
            o.UpdatedCheckpoints.Count == 1 &&
            o.UpdatedCheckpoints.First(a => a.Type == CheckpointType.Cashout).Stack == cashoutStack));
    }
    
    [Fact]
    public async Task AddsCheckpoint()
    {
        var cashoutStack = Create.Int();
        var startTime = DateTime.Parse("2001-01-01 12:00:00");
        var cashgameId = Create.String();
        var playerId = Create.String();
        var cashgame = Create.Cashgame(id: cashgameId);
        cashgame.SetCheckpoints([
            Create.BuyinAction(cashgameId: cashgameId, timestamp: startTime, stack: 200, buyin: 200)
        ]);
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);

        await ExecuteAsync(cashgameId, playerId, cashoutStack, startTime.AddMinutes(2));
        
        await _cashgameRepository.Received().Update(Arg.Is<Cashgame>(o =>
            o.AddedCheckpoints.Count == 1 &&
            o.AddedCheckpoints.First(a => a.Type == CheckpointType.Cashout).Stack == cashoutStack));
    }
    
    private async Task<UseCaseResult<Cashout.Result>> ExecuteAsync(string? cashgameId = null, string? playerId = null, int? stack = null, DateTime? cashoutTime = null)
    {
        return await Sut.Execute(new Cashout.Request(
            new AuthInTest(canEditCashgameActionsFor: true),
            cashgameId ?? Create.String(),
            playerId ?? Create.String(),
            stack ?? Create.Int(),
            cashoutTime ?? Create.DateTime()));
    }

    private Cashout Sut => new(_cashgameRepository);
}