using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class EditBunchTests : TestBase
{
    private const string Description = "description";
    private const string ValidCurrencySymbol = "symbol";
    private const string ValidCurrencyLayout = "layout";
    private const string ValidTimeZone = TestData.LocalTimeZoneName;
    private const string HouseRules = "houserules";
    private const int DefaultBuyin = 1;

    [Test]
    public async Task EditBunch_EmptyCurrencySymbol_ReturnsError()
    {
        var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, "", ValidCurrencyLayout, ValidTimeZone, HouseRules, DefaultBuyin);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditBunch_EmptyCurrencyLayout_ReturnsError()
    {
        var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, "", ValidTimeZone, HouseRules, DefaultBuyin);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditBunch_EmptyTimeZone_ReturnsError()
    {
        var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, "", HouseRules, DefaultBuyin);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    // todo: Have a look at this test. Unknown error can't be good
    [Test]
    public async Task EditBunch_InvalidTimeZone_ReturnsError()
    {
        var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, "a", HouseRules, DefaultBuyin);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Unknown));
    }

    [Test]
    public async Task EditBunch_ValidData_SavesBunch()
    {
        var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, ValidTimeZone, HouseRules, DefaultBuyin);

        await Sut.Execute(request);

        Assert.AreEqual(Description, Deps.Bunch.Saved.Description);
        Assert.AreEqual(ValidCurrencySymbol, Deps.Bunch.Saved.Currency.Symbol);
        Assert.AreEqual(ValidCurrencyLayout, Deps.Bunch.Saved.Currency.Layout);
        Assert.AreEqual(ValidTimeZone, Deps.Bunch.Saved.Timezone.Id);
        Assert.AreEqual(HouseRules, Deps.Bunch.Saved.HouseRules);
        Assert.AreEqual(DefaultBuyin, Deps.Bunch.Saved.DefaultBuyin);
    }

    [Test]
    public async Task EditBunch_ValidData_ReturnUrlIsCorrect()
    {
        var request = new EditBunch.Request(TestData.ManagerUser.UserName, TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, ValidTimeZone, HouseRules, DefaultBuyin);

        var result = await Sut.Execute(request);

        Assert.AreEqual("bunch-a", result.Data.Slug);
    }

    private EditBunch Sut => new(
        Deps.Bunch,
        Deps.User,
        Deps.Player);
}