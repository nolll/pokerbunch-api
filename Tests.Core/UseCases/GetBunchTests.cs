using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;
using Xunit;

namespace Tests.Core.UseCases;

public class GetBunchTests : TestBase
{
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    private readonly Bunch _bunch;
    
    public GetBunchTests()
    {
        _bunch = CreateBunch();
        _bunchRepository.GetBySlug(_bunch.Slug).Returns(_bunch);
    }
    
    [Fact]
    public async Task NoAccess_AccessDenied()
    {
        var result = await ExecuteAsync(false, _bunch.Slug);
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }

    [Fact]
    public async Task BunchNameIsSet()
    {
        var result = await ExecuteAsync(true, _bunch.Slug);
        result.Success.Should().BeTrue();
        result!.Data!.Name.Should().Be(_bunch.DisplayName);
        result!.Data!.Description.Should().Be(_bunch.Description);
        result!.Data!.HouseRules.Should().Be(_bunch.HouseRules);
    }

    private async Task<UseCaseResult<GetBunch.Result>> ExecuteAsync(bool canGetBunch, string slug)
    {
        return await Sut.Execute(
            new GetBunch.Request(new AuthInTest(canGetBunch: canGetBunch), slug));
    }

    private GetBunch Sut => new(_bunchRepository);
}