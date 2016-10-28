using System;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class EditBunchTests : TestBase
    {
        private const string Description = "description";
        private const string ValidCurrencySymbol = "symbol";
        private const string ValidCurrencyLayout = "layout";
        private const string ValidTimeZone = TestData.LocalTimeZoneName;
        private const string HouseRules = "houserules";
        private const int DefaultBuyin = 1;

        [Test]
        public void EditBunch_EmptyCurrencySymbol_ThrowsValidationException()
        {
            var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, "", ValidCurrencyLayout, ValidTimeZone, HouseRules, DefaultBuyin);

            Assert.Throws<ValidationException>(() => Sut.Execute(request));
        }

        [Test]
        public void EditBunch_EmptyCurrencyLayout_ThrowsValidationException()
        {
            var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, "", ValidTimeZone, HouseRules, DefaultBuyin);

            Assert.Throws<ValidationException>(() => Sut.Execute(request));
        }

        [Test]
        public void EditBunch_EmptyTimeZone_ThrowsValidationException()
        {
            var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, "", HouseRules, DefaultBuyin);

            Assert.Throws<ValidationException>(() => Sut.Execute(request));
        }

        [Test]
        public void EditBunch_InvalidTimeZone_ThrowsValidationException()
        {
            var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, "a", HouseRules, DefaultBuyin);

            Assert.Throws<TimeZoneNotFoundException>(() => Sut.Execute(request));
        }

        [Test]
        public void EditBunch_ValidData_SavesBunch()
        {
            var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, ValidTimeZone, HouseRules, DefaultBuyin);

            Sut.Execute(request);

            Assert.AreEqual(Description, Repos.Bunch.Saved.Description);
            Assert.AreEqual(ValidCurrencySymbol, Repos.Bunch.Saved.Currency.Symbol);
            Assert.AreEqual(ValidCurrencyLayout, Repos.Bunch.Saved.Currency.Layout);
            Assert.AreEqual(ValidTimeZone, Repos.Bunch.Saved.Timezone.Id);
            Assert.AreEqual(HouseRules, Repos.Bunch.Saved.HouseRules);
            Assert.AreEqual(DefaultBuyin, Repos.Bunch.Saved.DefaultBuyin);
        }

        [Test]
        public void EditBunch_ValidData_ReturnUrlIsCorrect()
        {
            var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, ValidTimeZone, HouseRules, DefaultBuyin);

            var result = Sut.Execute(request);

            Assert.AreEqual("bunch-a", result.Slug);
        }

        private EditBunch Sut => new EditBunch(
            Services.BunchService,
            Repos.User,
            Services.PlayerService);
    }
}