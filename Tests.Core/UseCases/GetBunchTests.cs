using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class GetBunchTests : TestBase
{
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    private readonly Bunch _bunch;
    
    public GetBunchTests()
    {
        _bunch = Create.Bunch();
        _bunchRepository.GetBySlug(_bunch.Slug).Returns(_bunch);
    }
    
    [Fact]
    public async Task NoAccess_AccessDenied()
    {
        var request = CreateRequest(_bunch.Slug, canGetBunch: false);
        var result = await Sut.Execute(request);
        
        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }

    [Fact]
    public async Task BunchNameIsSet()
    {
        var request = CreateRequest(_bunch.Slug);
        var result = await Sut.Execute(request);
        
        result.Success.Should().BeTrue();
        result.Data!.Name.Should().Be(_bunch.DisplayName);
        result.Data!.Description.Should().Be(_bunch.Description);
        result.Data!.HouseRules.Should().Be(_bunch.HouseRules);
    }

    private GetBunch.Request CreateRequest(string slug, bool? canGetBunch = null)
    {
        return new GetBunch.Request(
            new AuthInTest(canGetBunch: canGetBunch ?? true), 
            slug);
    }

    private GetBunch Sut => new(_bunchRepository);
}