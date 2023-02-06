using Core.Entities;
using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

class AddBunchTests : TestBase
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
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task AddBunch_WithEmptyCurrencySymbol_ReturnsValidationError()
    {
        var result = await Sut.Execute(CreateRequest(currencySymbol: ""));
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task AddBunch_WithEmptyCurrencyLayout_ReturnsValidationError()
    {
        var result = await Sut.Execute(CreateRequest(currencyLayout: ""));
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task AddBunch_WithEmptyTimeZone_ReturnsValidationError()
    {
        var result = await Sut.Execute(CreateRequest(timeZone: ""));
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task AddBunch_WithExistingSlug_ReturnsConflictError()
    {
        var result = await Sut.Execute(CreateRequest(_existingDisplayName));
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Conflict));
    }

    [Test]
    public async Task AddBunch_WithGoodInput_CreatesBunch()
    {
        await Sut.Execute(CreateRequest());

        Assert.That(Deps.Bunch.Added?.Id, Is.Null);
        Assert.That(Deps.Bunch.Added?.Slug, Is.EqualTo("a-display-name"));
        Assert.That(Deps.Bunch.Added?.DisplayName, Is.EqualTo(DisplayName));
        Assert.That(Deps.Bunch.Added?.Description, Is.EqualTo(Description));
        Assert.That(Deps.Bunch.Added?.HouseRules, Is.EqualTo(""));
        Assert.That(Deps.Bunch.Added?.Timezone.Id, Is.EqualTo(TestData.TimeZoneLocal.Id));
        Assert.That(Deps.Bunch.Added?.DefaultBuyin, Is.EqualTo(0));
        Assert.That(Deps.Bunch.Added?.Currency.Symbol, Is.EqualTo(CurrencySymbol));
        Assert.That(Deps.Bunch.Added?.Currency.Layout, Is.EqualTo(CurrencyLayout));
    }

    [Test]
    public async Task AddBunch_WithGoodInput_CreatesPlayer()
    {
        await Sut.Execute(CreateRequest());

        Assert.That(Deps.Player.Added?.BunchId, Is.EqualTo("1"));
        Assert.That(Deps.Player.Added?.UserId, Is.EqualTo("3"));
        Assert.That(Deps.Player.Added?.Role, Is.EqualTo(Role.Manager));
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