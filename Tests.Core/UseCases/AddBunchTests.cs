using Core.Entities;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class AddBunchTests : TestBase
{
    private const string DisplayName = "A Display Name";
    private const string Description = "b";
    private const string CurrencySymbol = "c";
    private const string CurrencyLayout = "d";
    private readonly string _existingDisplayName = TestData.BunchA.DisplayName;

    private string _timeZone;

    [SetUp]
    public void SetUp()
    {
        _timeZone = TestData.LocalTimeZoneName;
    }
        
    [Test]
    public void AddBunch_WithEmptyDisplayName_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() => Sut.Execute(CreateRequest("")));
    }

    [Test]
    public void AddBunch_WithEmptyCurrencySymbol_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() => Sut.Execute(CreateRequest(currencySymbol: "")));
    }

    [Test]
    public void AddBunch_WithEmptyCurrencyLayout_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() => Sut.Execute(CreateRequest(currencyLayout: "")));
    }

    [Test]
    public void AddBunch_WithEmptyTimeZone_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() => Sut.Execute(CreateRequest(timeZone: "")));
    }

    [Test]
    public void AddBunch_WithExistingSlug_ThrowsException()
    {
        Assert.Throws<BunchExistsException>(() => Sut.Execute(CreateRequest(_existingDisplayName)));
    }

    [Test]
    public void AddBunch_WithGoodInput_CreatesBunch()
    {
        Sut.Execute(CreateRequest());

        Assert.AreEqual(0, Deps.Bunch.Added.Id);
        Assert.AreEqual("a-display-name", Deps.Bunch.Added.Slug);
        Assert.AreEqual(DisplayName, Deps.Bunch.Added.DisplayName);
        Assert.AreEqual(Description, Deps.Bunch.Added.Description);
        Assert.AreEqual("", Deps.Bunch.Added.HouseRules);
        Assert.AreEqual(TestData.TimeZoneLocal.Id, Deps.Bunch.Added.Timezone.Id);
        Assert.AreEqual(0, Deps.Bunch.Added.DefaultBuyin);
        Assert.AreEqual(CurrencySymbol, Deps.Bunch.Added.Currency.Symbol);
        Assert.AreEqual(CurrencyLayout, Deps.Bunch.Added.Currency.Layout);
    }

    [Test]
    public void AddBunch_WithGoodInput_CreatesPlayer()
    {
        Sut.Execute(CreateRequest());

        Assert.AreEqual(1, Deps.Player.Added.BunchId);
        Assert.AreEqual(3, Deps.Player.Added.UserId);
        Assert.AreEqual(Role.Manager, Deps.Player.Added.Role);
    }

    private AddBunch.Request CreateRequest(string displayName = DisplayName, string currencySymbol = CurrencySymbol, string currencyLayout = CurrencyLayout, string timeZone = null)
    {
        return new AddBunch.Request(TestData.UserNameC, displayName, Description, currencySymbol, currencyLayout, timeZone ?? _timeZone);
    }

    private AddBunch Sut => new AddBunch(
        Deps.User,
        Deps.Bunch,
        Deps.Player);
}