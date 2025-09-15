using Core.Entities;
using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class AddBunchTests : TestBase
{
    private const string DisplayName = "A Display Name";
    private const string Description = "b";
    private const string CurrencySymbol = "c";
    private const string CurrencyLayout = "d";
    private readonly string _existingDisplayName = TestData.BunchA.DisplayName;

    private string _timeZone = TestData.LocalTimeZoneName;

    [SetUp]
    public void SetUp()
    {
        _timeZone = TestData.LocalTimeZoneName;
    }
        
    [Test]
    public async Task AddBunch_WithEmptyDisplayName_ReturnsValidationError()
    {
        var result = await Sut.Execute(CreateRequest(""));
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task AddBunch_WithEmptyCurrencySymbol_ReturnsValidationError()
    {
        var result = await Sut.Execute(CreateRequest(currencySymbol: ""));
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task AddBunch_WithEmptyCurrencyLayout_ReturnsValidationError()
    {
        var result = await Sut.Execute(CreateRequest(currencyLayout: ""));
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task AddBunch_WithEmptyTimeZone_ReturnsValidationError()
    {
        var result = await Sut.Execute(CreateRequest(timeZone: ""));
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task AddBunch_WithExistingSlug_ReturnsConflictError()
    {
        var result = await Sut.Execute(CreateRequest(_existingDisplayName));
        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Test]
    public async Task AddBunch_WithGoodInput_CreatesBunch()
    {
        await Sut.Execute(CreateRequest());

        Deps.Bunch.Added!.Id.Should().Be("");
        Deps.Bunch.Added!.Slug.Should().Be("a-display-name");
        Deps.Bunch.Added!.DisplayName.Should().Be(DisplayName);
        Deps.Bunch.Added!.Description.Should().Be(Description);
        Deps.Bunch.Added!.HouseRules.Should().Be("");
        Deps.Bunch.Added!.Timezone.Id.Should().Be(TestData.TimeZoneLocal.Id);
        Deps.Bunch.Added!.DefaultBuyin.Should().Be(0);
        Deps.Bunch.Added!.Currency.Symbol.Should().Be(CurrencySymbol);
        Deps.Bunch.Added!.Currency.Layout.Should().Be(CurrencyLayout);
    }

    [Test]
    public async Task AddBunch_WithGoodInput_CreatesPlayer()
    {
        await Sut.Execute(CreateRequest());

        Deps.Player.Added!.BunchId.Should().Be("1");
        Deps.Player.Added!.UserId.Should().Be("3");
        Deps.Player.Added!.Role.Should().Be(Role.Manager);
    }

    private AddBunch.Request CreateRequest(string displayName = DisplayName, string currencySymbol = CurrencySymbol, string currencyLayout = CurrencyLayout, string? timeZone = null)
    {
        return new AddBunch.Request(TestData.UserNameC, displayName, Description, currencySymbol, currencyLayout, timeZone ?? _timeZone);
    }

    private AddBunch Sut => new(
        Deps.User,
        Deps.Bunch,
        Deps.Player);
}