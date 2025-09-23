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
        var request = CreateRequest(stack: -1);
        var result = await Sut.Execute(request);
        
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

        var request = CreateRequest(cashgame.Id, playerId, cashoutStack, startTime.AddMinutes(2));
        await Sut.Execute(request);
        
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

        var request = CreateRequest(cashgameId, playerId, cashoutStack, startTime.AddMinutes(2));
        await Sut.Execute(request);
        
        await _cashgameRepository.Received().Update(Arg.Is<Cashgame>(o =>
            o.AddedCheckpoints.Count == 1 &&
            o.AddedCheckpoints.First(a => a.Type == CheckpointType.Cashout).Stack == cashoutStack));
    }
    
    private Cashout.Request CreateRequest(string? cashgameId = null, string? playerId = null, int? stack = null, DateTime? cashoutTime = null)
    {
        return new Cashout.Request(
            new AuthInTest(canEditCashgameActionsFor: true),
            cashgameId ?? Create.String(),
            playerId ?? Create.String(),
            stack ?? Create.Int(),
            cashoutTime ?? Create.DateTime());
    }

    private Cashout Sut => new(_cashgameRepository);
}