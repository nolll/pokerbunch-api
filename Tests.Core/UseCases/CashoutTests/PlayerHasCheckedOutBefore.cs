using System.Linq;
using Core.Entities.Checkpoints;
using Core.UseCases;
using NUnit.Framework;

namespace Tests.Core.UseCases.CashoutTests
{
    public class PlayerHasCheckedOutBefore : Arrange
    {
        protected override bool HasCashedOutBefore => true;

        [Test]
        public void UpdatesCheckpoint()
        {
            Sut.Execute(new Cashout.Request(UserName, Slug, PlayerId, CashoutStack, CashoutTime));

            Assert.AreEqual(CheckpointCountBeforeCashout, UpdatedCashgame.Checkpoints.Count);
            Assert.IsTrue(UpdatedCashgame.UpdatedCheckpoints.First(o => o.Type == CheckpointType.Cashout).Stack == CashoutStack);
        }
    }
}
