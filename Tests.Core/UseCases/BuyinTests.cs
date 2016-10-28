using System;
using System.Linq;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
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
            var request = new Buyin.Request(TestData.UserNameA, TestData.SlugA, PlayerId, InvalidBuyin, ValidStack, DateTime.UtcNow);

            var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
            Assert.AreEqual(1, ex.Messages.Count());
        }

        [Test]
        public void Buyin_InvalidStackSize_ReturnsError()
        {
            var request = new Buyin.Request(TestData.UserNameA, TestData.SlugA, PlayerId, ValidBuyin, InvalidStack, DateTime.UtcNow);

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

            Repos.Cashgame.SetupRunningGame();

            var request = new Buyin.Request(TestData.UserNameA, TestData.SlugA, PlayerId, buyin, stack, timestamp);
            Sut.Execute(request);

            var result = Repos.Cashgame.Updated.AddedCheckpoints.First();

            Assert.AreEqual(timestamp, result.Timestamp);
            Assert.AreEqual(buyin, result.Amount);
            Assert.AreEqual(savedStack, result.Stack);
        }

        private Buyin Sut => new Buyin(
            Services.BunchService,
            Services.PlayerService,
            Services.CashgameService,
            Repos.User);
    }
}
