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

    [Fact]
    public async Task AddBunch_WithEmptyDisplayName_ReturnsValidationError()
    {
        var request = CreateRequest(displayName: "");
        var result = await Sut.Execute(request);
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddBunch_WithEmptyCurrencySymbol_ReturnsValidationError()
    {
        var request = CreateRequest(currencySymbol: "");
        var result = await Sut.Execute(request);
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddBunch_WithEmptyCurrencyLayout_ReturnsValidationError()
    {
        var request = CreateRequest(currencyLayout: "");
        var result = await Sut.Execute(request);
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddBunch_WithEmptyTimeZone_ReturnsValidationError()
    {
        var request = CreateRequest(timeZone: "");
        var result = await Sut.Execute(request);
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddBunch_WithExistingSlug_ReturnsConflictError()
    {
        var existingBunch = Create.Bunch(slug: Slug);
        _bunchRepository.GetBySlugOrNull(Slug).Returns(existingBunch);
     
        var request = CreateRequest(displayName: DisplayName);
        var result = await Sut.Execute(request);
        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task AddBunch_WithGoodInput_CreatesBunchAndAddsPlayer()
    {
        var auth = new AuthInTest(id: Create.String(), userName: Create.String());
        var description = Create.String();
        var currencySymbol = Create.String();
        var currencyLayout = Create.String();
        var timeZone = Create.TimeZoneId();
        
        var request = CreateRequest(
            auth: auth, 
            displayName: DisplayName,
            description: description,
            timeZone: timeZone,
            currencySymbol: currencySymbol,
            currencyLayout: currencyLayout);
        
        await Sut.Execute(request);

        await _bunchRepository.Received().Add(Arg.Is<Bunch>(o =>
            o.Slug == Slug &&
            o.DisplayName == DisplayName &&
            o.Description == description &&
            o.HouseRules == "" &&
            o.DefaultBuyin == 0 &&
            o.Timezone.Id == timeZone &&
            o.Currency.Symbol == currencySymbol &&
            o.Currency.Layout == currencyLayout));
        
        await _playerRepository.Received().Add(Arg.Is<Player>(o =>
            o.UserId == auth.Id &&
            o.Role == Role.Manager));
    }

    private AddBunch.Request CreateRequest(
        IAuth? auth = null,
        string? displayName = null,
        string? description = null,
        string? currencySymbol = null,
        string? currencyLayout = null,
        string? timeZone = null) =>
        new(
            auth ?? new AuthInTest(),
            displayName ?? Create.String(),
            description ?? Create.String(),
            currencySymbol ?? Create.String(),
            currencyLayout ?? Create.String(),
            timeZone ?? Create.TimeZoneId());

    private AddBunch Sut => new(_bunchRepository, _playerRepository);
}