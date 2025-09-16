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
using Xunit;

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
        var cashoutStack = Fixture.Create<int>();
        var startTime = DateTime.Parse("2001-01-01 12:00:00");
        var cashgameId = Fixture.Create<string>();
        var playerId = Fixture.Create<string>();
        List<Checkpoint> checkpoints =
        [
            Checkpoint.Create("1", cashgameId, playerId, startTime, CheckpointType.Buyin, 200, 200),
            Checkpoint.Create("3", cashgameId, playerId, startTime.AddMinutes(1), CheckpointType.Cashout, 200, 0)
        ];
        var cashgame = CreateCashgame(id: cashgameId, checkpoints: checkpoints);
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);

        await ExecuteAsync(cashgameId, playerId, cashoutStack, startTime.AddMinutes(2));
        
        await _cashgameRepository.Received().Update(Arg.Is<Cashgame>(o =>
            o.UpdatedCheckpoints.Count == 1 &&
            o.UpdatedCheckpoints.First(a => a.Type == CheckpointType.Cashout).Stack == cashoutStack));
    }
    
    [Fact]
    public async Task AddsCheckpoint()
    {
        var cashoutStack = Fixture.Create<int>();
        var startTime = DateTime.Parse("2001-01-01 12:00:00");
        var cashgameId = Fixture.Create<string>();
        var playerId = Fixture.Create<string>();
        List<Checkpoint> checkpoints =
        [
            Checkpoint.Create("1", cashgameId, playerId, startTime, CheckpointType.Buyin, 200, 200)
        ];
        var cashgame = CreateCashgame(id: cashgameId, checkpoints: checkpoints);
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
            cashgameId ?? Fixture.Create<string>(),
            playerId ?? Fixture.Create<string>(),
            stack ?? Fixture.Create<int>(),
            cashoutTime ?? Fixture.Create<DateTime>()));
    }

    private Cashout Sut => new(_cashgameRepository);
}