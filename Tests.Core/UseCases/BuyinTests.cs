using System;
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
    private const int ValidBuyin = 1;
    private const int InvalidBuyin = 0;
    private const int ValidStack = 0;
    private const int InvalidStack = -1;
    
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();

    [Test]
    public async Task Buyin_InvalidBuyin_ReturnsError()
    {
        var request = new Buyin.Request(new AuthInTest(canEditCashgameActionsFor: true), TestData.CashgameIdA, PlayerId, InvalidBuyin, ValidStack, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task Buyin_InvalidStackSize_ReturnsError()
    {
        var request = new Buyin.Request(new AuthInTest(canEditCashgameActionsFor: true), TestData.CashgameIdA, PlayerId, ValidBuyin, InvalidStack, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task Buyin_StartedCashgame_AddsCheckpointWithCorrectValues()
    {
        var timestamp = DateTime.UtcNow;
        const int buyin = 1;
        const int stack = 2;
        const int savedStack = 3;

        var cashgame = Create.Cashgame(status: GameStatus.Running);
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);

        var request = new Buyin.Request(new AuthInTest(canEditCashgameActionsFor: true), cashgame.Id, PlayerId, buyin, stack, timestamp);
        await Sut.Execute(request);
        
        await _cashgameRepository.Received()
            .Update(Arg.Is<Cashgame>(o => o.AddedCheckpoints.Count == 1 &&
                                          o.AddedCheckpoints.First().Timestamp == timestamp &&
                                          o.AddedCheckpoints.First().Amount == buyin &&
                                          o.AddedCheckpoints.First().Stack == savedStack));
    }

    private Buyin Sut => new(_cashgameRepository);
}