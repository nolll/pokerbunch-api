using System;
using System.Linq;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class BuyinTests : TestBase
{
    private const int PlayerId = 1;
    private const int ValidBuyin = 1;
    private const int InvalidBuyin = 0;
    private const int ValidStack = 0;
    private const int InvalidStack = -1;

    [Test]
    public void Buyin_InvalidBuyin_ReturnsError()
    {
        var request = new Buyin.Request(TestData.UserNameA, TestData.CashgameIdA, PlayerId, InvalidBuyin, ValidStack, DateTime.UtcNow);

        var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
        Assert.AreEqual(1, ex.Messages.Count());
    }

    [Test]
    public void Buyin_InvalidStackSize_ReturnsError()
    {
        var request = new Buyin.Request(TestData.UserNameA, TestData.CashgameIdA, PlayerId, ValidBuyin, InvalidStack, DateTime.UtcNow);

        var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
        Assert.AreEqual(1, ex.Messages.Count());
    }

    [Test]
    public void Buyin_StartedCashgame_AddsCheckpointWithCorrectValues()
    {
        var timestamp = DateTime.UtcNow;
        const int buyin = 1;
        const int stack = 2;
        const int savedStack = 3;

        Deps.Cashgame.SetupRunningGame();

        var request = new Buyin.Request(TestData.UserNameA, TestData.CashgameIdA, PlayerId, buyin, stack, timestamp);
        Sut.Execute(request);

        var result = Deps.Cashgame.Updated.AddedCheckpoints.First();

        Assert.AreEqual(timestamp, result.Timestamp);
        Assert.AreEqual(buyin, result.Amount);
        Assert.AreEqual(savedStack, result.Stack);
    }

    private Buyin Sut => new Buyin(
        Deps.Cashgame,
        Deps.Player,
        Deps.User);
}