using System.Linq;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class DeleteCheckpointTests : TestBase
    {
        [Test]
        public void DeleteCheckpoint_EndedGame_DeletesCheckpointAndReturnsCorrectValues()
        {
            var request = new DeleteCheckpoint.Request(TestData.ManagerUser.UserName, TestData.ReportCheckpointId);
            var result = Sut.Execute(request);

            var deletedCheckpointIds = Repos.Cashgame.Updated.DeletedCheckpoints.Select(o => o.Id);
            Assert.IsTrue(deletedCheckpointIds.Contains(TestData.ReportCheckpointId));
            Assert.AreEqual("bunch-a", result.Slug);
            Assert.AreEqual(1, result.CashgameId);
            Assert.IsFalse(result.GameIsRunning);
        }

        [Test]
        public void DeleteCheckpoint_RunningGame_DeletesCheckpointAndReturnsCorrectValues()
        {
            Repos.Cashgame.SetupRunningGame();

            var request = new DeleteCheckpoint.Request(TestData.ManagerUser.UserName, 12);
            var result = Sut.Execute(request);

            var deletedCheckpointIds = Repos.Cashgame.Updated.DeletedCheckpoints.Select(o => o.Id);
            Assert.IsTrue(deletedCheckpointIds.Contains(12));
            Assert.AreEqual("bunch-a", result.Slug);
            Assert.AreEqual(3, result.CashgameId);
            Assert.IsTrue(result.GameIsRunning);
        }

        private DeleteCheckpoint Sut => new DeleteCheckpoint(
            Repos.Bunch,
            Services.CashgameService,
            Repos.User,
            Services.PlayerService);
    }
}