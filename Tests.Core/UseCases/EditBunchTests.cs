using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class EditBunchTests : TestBase
{
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    
    [Fact]
    public async Task EditBunch_EmptyCurrencySymbol_ReturnsError()
    {
        var request = CreateRequest(currencySymbol: "");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task EditBunch_EmptyCurrencyLayout_ReturnsError()
    {
        var request = CreateRequest(currencyLayout: "");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    public async Task EditBunch_InvalidTimeZone_ReturnsError(string timeZone)
    {
        var request = CreateRequest(timeZone: timeZone);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task EditBunch_NoAccess_ReturnsError()
    {
        var bunch = Create.Bunch();
        _bunchRepository.GetBySlug(bunch.Slug).Returns(bunch);
        
        var request = CreateRequest(slug: bunch.Slug, canEditBunch: false);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }

    [Fact]
    public async Task EditBunch_ValidData_SavesBunch()
    {
        var bunch = Create.Bunch();
        _bunchRepository.GetBySlug(bunch.Slug).Returns(bunch);

        var description = Create.String();
        var currencySymbol = Create.String();
        var currencyLayout = Create.String();
        var timeZone = Create.TimeZoneId();
        var houseRules = Create.String();
        var defaultBuyin = Create.Int();
        
        var request = CreateRequest(bunch.Slug, description, currencySymbol, currencyLayout, timeZone, houseRules, defaultBuyin);

        var result = await Sut.Execute(request);

        await _bunchRepository.Received()
            .Update(Arg.Is<Bunch>(o => o.Description == description && 
                                       o.Currency.Symbol == currencySymbol && 
                                       o.Currency.Layout == currencyLayout &&
                                       o.Timezone.Id == timeZone &&
                                       o.HouseRules == houseRules &&
                                       o.DefaultBuyin == defaultBuyin));
        
        result.Data!.Slug.Should().Be(bunch.Slug);
    }

    private EditBunch.Request CreateRequest(
        string? slug = null,
        string? description = null,
        string? currencySymbol = null,
        string? currencyLayout = null,
        string? timeZone = null,
        string? houseRules = null,
        int? defaultBuyin = null,
        bool? canEditBunch = null)
    {
        return new EditBunch.Request(
            new AuthInTest(canEditBunch: canEditBunch ?? true), 
            slug ?? Create.String(), 
            description ?? Create.String(), 
            currencySymbol ?? Create.String(), 
            currencyLayout ?? Create.String(), 
            timeZone ?? Create.TimeZoneId(), 
            houseRules ?? Create.String(), 
            defaultBuyin ?? Create.Int());
    }

    private EditBunch Sut => new(_bunchRepository);
}