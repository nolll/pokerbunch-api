using Core.Errors;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

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
        var request = new EditBunch.Request(new AuthInTest(canEditBunch: true), TestData.SlugA, Description, "", ValidCurrencyLayout, ValidTimeZone, HouseRules, DefaultBuyin);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditBunch_EmptyCurrencyLayout_ReturnsError()
    {
        var request = new EditBunch.Request(new AuthInTest(canEditBunch: true), TestData.SlugA, Description, ValidCurrencySymbol, "", ValidTimeZone, HouseRules, DefaultBuyin);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditBunch_EmptyTimeZone_ReturnsError()
    {
        var request = new EditBunch.Request(new AuthInTest(canEditBunch: true), TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, "", HouseRules, DefaultBuyin);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditBunch_InvalidTimeZone_ReturnsError()
    {
        var request = new EditBunch.Request(new AuthInTest(canEditBunch: true), TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, "invalid", HouseRules, DefaultBuyin);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditBunch_ValidData_SavesBunch()
    {
        var request = new EditBunch.Request(new AuthInTest(canEditBunch: true), TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, ValidTimeZone, HouseRules, DefaultBuyin);

        await Sut.Execute(request);

        Deps.Bunch.Saved?.Description.Should().Be(Description);
        Deps.Bunch.Saved?.Currency.Symbol.Should().Be(ValidCurrencySymbol);
        Deps.Bunch.Saved?.Currency.Layout.Should().Be(ValidCurrencyLayout);
        Deps.Bunch.Saved?.Timezone.Id.Should().Be(ValidTimeZone);
        Deps.Bunch.Saved?.HouseRules.Should().Be(HouseRules);
        Deps.Bunch.Saved?.DefaultBuyin.Should().Be(DefaultBuyin);
    }

    [Test]
    public async Task EditBunch_ValidData_ReturnUrlIsCorrect()
    {
        var request = new EditBunch.Request(new AuthInTest(canEditBunch: true), TestData.SlugA, Description, ValidCurrencySymbol, ValidCurrencyLayout, ValidTimeZone, HouseRules, DefaultBuyin);

        var result = await Sut.Execute(request);

        result.Data?.Slug.Should().Be("bunch-a");
    }

    private EditBunch Sut => new(Deps.Bunch);
}