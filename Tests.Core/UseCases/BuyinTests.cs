using System;
using System.Runtime.InteropServices.JavaScript;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class BuyinTests : TestBase
{
    private const string PlayerId = "1";
    
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Buyin_InvalidBuyin_ReturnsError(int buyin)
    {
        var request = CreateRequest(buyin: buyin);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Buyin_InvalidStackSize_ReturnsError()
    {
        var request = CreateRequest(stack: -1);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Buyin_StartedCashgame_AddsCheckpointWithCorrectValues()
    {
        var timestamp = Create.DateTime();
        var buyin = Create.Int();
        var stack = Create.Int();
        var savedStack = buyin + stack;

        var cashgame = Create.Cashgame(status: GameStatus.Running);
        var player = Create.Player();
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);

        var request = CreateRequest(cashgameId: cashgame.Id, playerId: player.Id, buyin: buyin, stack: stack, timestamp: timestamp);
        await Sut.Execute(request);
        
        await _cashgameRepository.Received()
            .Update(Arg.Is<Cashgame>(o => o.AddedCheckpoints.Count == 1 &&
                                          o.AddedCheckpoints.First().Timestamp == timestamp &&
                                          o.AddedCheckpoints.First().Amount == buyin &&
                                          o.AddedCheckpoints.First().Stack == savedStack));
    }

    private Buyin.Request CreateRequest(
        string? cashgameId = null,
        string? playerId = null,
        int? buyin = null,
        int? stack = null,
        DateTime? timestamp = null,
        bool? canEditCashgameActionsFor = null)
    {
        return new Buyin.Request(
            new AuthInTest(
                canEditCashgameActionsFor: canEditCashgameActionsFor ?? true), 
            cashgameId ?? Create.String(), 
            playerId ?? Create.String(),
            buyin ?? Create.Int(),
            stack ?? Create.Int(),
            timestamp ?? Create.DateTime());
    }

    private Buyin Sut => new(_cashgameRepository);
}