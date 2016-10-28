using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class EditCashgameFormTests : TestBase
    {
        [Test]
        public void EditCashgameForm_AllPropertiesAreSet()
        {
            var result = Sut.Execute(new EditCashgameForm.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA));

            Assert.AreEqual(1, result.CashgameId);
            Assert.AreEqual(TestData.LocationIdA, result.LocationId);
            Assert.AreEqual(TestData.DateStringA, result.Date);
        }
        
        [Test]
        public void EditCashgameForm_LocationsAreSet()
        {
            var result = Sut.Execute(new EditCashgameForm.Request(TestData.ManagerUser.UserName, TestData.CashgameIdA));

            Assert.AreEqual(4, result.Locations.Count);
        }

        private EditCashgameForm Sut => new EditCashgameForm(
            Repos.Bunch,
            Services.CashgameService,
            Repos.User,
            Services.PlayerService,
            Repos.Location,
            Repos.Event);
    }
}