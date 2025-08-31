using System;
using Core.Errors;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

class BuyinTests : TestBase
{
    private const string PlayerId = "1";
    private const int ValidBuyin = 1;
    private const int InvalidBuyin = 0;
    private const int ValidStack = 0;
    private const int InvalidStack = -1;

    [Test]
    public async Task Buyin_InvalidBuyin_ReturnsError()
    {
        var request = new Buyin.Request(new AccessControlInTest(canEditCashgameActionsFor: true), TestData.CashgameIdA, PlayerId, InvalidBuyin, ValidStack, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task Buyin_InvalidStackSize_ReturnsError()
    {
        var request = new Buyin.Request(new AccessControlInTest(canEditCashgameActionsFor: true), TestData.CashgameIdA, PlayerId, ValidBuyin, InvalidStack, DateTime.UtcNow);
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task Buyin_StartedCashgame_AddsCheckpointWithCorrectValues()
    {
        var timestamp = DateTime.UtcNow;
        const int buyin = 1;
        const int stack = 2;
        const int savedStack = 3;

        Deps.Cashgame.SetupRunningGame();

        var request = new Buyin.Request(new AccessControlInTest(canEditCashgameActionsFor: true), TestData.CashgameIdA, PlayerId, buyin, stack, timestamp);
        await Sut.Execute(request);

        var result = Deps.Cashgame.Updated?.AddedCheckpoints.First();

        Assert.That(result?.Timestamp, Is.EqualTo(timestamp));
        Assert.That(result?.Amount, Is.EqualTo(buyin));
        Assert.That(result?.Stack, Is.EqualTo(savedStack));
    }

    private Buyin Sut => new(Deps.Cashgame);
}