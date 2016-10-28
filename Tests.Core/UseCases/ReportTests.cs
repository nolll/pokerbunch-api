using System;
using System.Linq;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class ReportTests : TestBase
    {
        [Test]
        public void Cashout_InvalidStack_ThrowsValidationException()
        {
            Repos.Cashgame.SetupRunningGame();

            var request = new Report.Request(TestData.UserNameA, TestData.SlugA, TestData.PlayerIdA, -1, DateTime.Now);

            Assert.Throws<ValidationException>(() => Sut.Execute(request));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Cashout_PlayerHasNotCheckedOutBefore_AddsCheckpoint(int stack)
        {
            Repos.Cashgame.SetupRunningGame();

            var request = new Report.Request(TestData.UserNameA, TestData.SlugA, TestData.PlayerIdA, stack, DateTime.Now);
            Sut.Execute(request);

            var result = Repos.Cashgame.Updated.AddedCheckpoints.First();
            Assert.AreEqual(stack, result.Stack);
        }

        private Report Sut => new Report(
            Repos.Bunch,
            Repos.Cashgame,
            Repos.Player,
            Repos.User);
    }
}