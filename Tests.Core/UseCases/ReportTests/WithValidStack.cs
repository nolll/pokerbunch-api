using System.Linq;
using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.ReportTests
{
    public class WithValidStack : Arrange
    {
        protected override bool ExecuteAutomatically => false;
        private Cashgame UpdatedCashgame;

        [Test]
        public void AddsCheckpoint(int stack)
        {
            var addedCheckpoint = UpdatedCashgame.AddedCheckpoints.First();
            Assert.AreEqual(stack, addedCheckpoint.Stack);
        }
    }
}