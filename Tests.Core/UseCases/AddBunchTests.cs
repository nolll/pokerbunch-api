using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class AddBunchTests : TestBase
{
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();
    
    private const string DisplayName = "A Display Name";
    private const string Slug = "a-display-name";

    private const string TimeZone = TestData.LocalTimeZoneName;

    [Fact]
    public async Task AddBunch_WithEmptyDisplayName_ReturnsValidationError()
    {
        var result = await ExecuteAsync(displayName: "");
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddBunch_WithEmptyCurrencySymbol_ReturnsValidationError()
    {
        var result = await ExecuteAsync(currencySymbol: "");
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddBunch_WithEmptyCurrencyLayout_ReturnsValidationError()
    {
        var result = await ExecuteAsync(currencyLayout: "");
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddBunch_WithEmptyTimeZone_ReturnsValidationError()
    {
        var result = await ExecuteAsync(timeZone: "");
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddBunch_WithExistingSlug_ReturnsConflictError()
    {
        var existingBunch = Create.Bunch(slug: Slug);
        _bunchRepository.GetBySlugOrNull(Slug).Returns(existingBunch);
        
        var result = await ExecuteAsync(displayName: DisplayName);
        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task AddBunch_WithGoodInput_CreatesBunchAndAddsPlayer()
    {
        var auth = new AuthInTest(id: Create.String(), userName: Create.String());
        var description = Create.String();
        var currencySymbol = Create.String();
        var currencyLayout = Create.String();
        
        await ExecuteAsync(
            auth: auth, 
            displayName: DisplayName,
            description: description,
            timeZone: TimeZone,
            currencySymbol: currencySymbol,
            currencyLayout: currencyLayout);

        await _bunchRepository.Received().Add(Arg.Is<Bunch>(o =>
            o.Slug == Slug &&
            o.DisplayName == DisplayName &&
            o.Description == description &&
            o.HouseRules == "" &&
            o.DefaultBuyin == 0 &&
            o.Timezone.Id == TimeZone &&
            o.Currency.Symbol == currencySymbol &&
            o.Currency.Layout == currencyLayout));
        
        await _playerRepository.Received().Add(Arg.Is<Player>(o =>
            o.UserId == auth.Id &&
            o.Role == Role.Manager));
    }
    
    private Task<UseCaseResult<AddBunch.Result>> ExecuteAsync(
        IAuth? auth = null,
        string? displayName = null, 
        string? description = null,
        string? currencySymbol = null, 
        string? currencyLayout = null,
        string? timeZone = null)
    {
        var request = new AddBunch.Request(
            auth ?? new AuthInTest(),
            displayName ?? Create.String(), 
            description ?? Create.String(), 
            currencySymbol ?? Create.String(), 
            currencyLayout ?? Create.String(), 
            timeZone ?? TimeZone);
        return Sut.Execute(request);
    }

    private AddBunch Sut => new(_bunchRepository, _playerRepository);
}