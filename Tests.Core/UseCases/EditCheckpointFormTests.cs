using System;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class EditCheckpointFormTests : TestBase
    {
        [Test]
        public void EditCheckpointForm_StackAndAmountAndTimestampIsSet()
        {
            var expected = DateTime.Parse("2001-01-01 11:30:00");

            var result = Sut.Execute(CreateRequest(3));

            Assert.AreEqual(250, result.Stack);
            Assert.AreEqual(0, result.Amount);
            Assert.AreEqual(expected, result.TimeStamp);
        }

        [Test]
        public void EditCheckpointForm_CheckpointIdIsSet()
        {
            var result = Sut.Execute(CreateRequest(TestData.ReportCheckpointId));

            Assert.AreEqual(2, result.CheckpointId);
        }

        [Test]
        public void EditCheckpointForm_CancelUrlIsSet()
        {
            var result = Sut.Execute(CreateRequest(TestData.ReportCheckpointId));

            Assert.AreEqual(1, result.CashgameId);
            Assert.AreEqual(3, result.PlayerId);
        }

        [Test]
        public void EditCheckpointForm_WithBuyinCheckpoint_CanEditAmountIsTrue()
        {
            var result = Sut.Execute(CreateRequest(TestData.BuyinCheckpointId));

            Assert.IsTrue(result.CanEditAmount);
        }

        [TestCase(3)]
        [TestCase(5)]
        public void EditCheckpointForm_WithOtherCheckpointType_CanEditAmountIsFalse(int id)
        {
            var result = Sut.Execute(CreateRequest(id));

            Assert.IsFalse(result.CanEditAmount);
        }

        private static EditCheckpointForm.Request CreateRequest(int id)
        {
            return new EditCheckpointForm.Request(TestData.ManagerUser.UserName, id);
        }

        private EditCheckpointForm Sut => new EditCheckpointForm(
            Deps.Bunch,
            Deps.Cashgame,
            Deps.User,
            Deps.Player);
    }
}
