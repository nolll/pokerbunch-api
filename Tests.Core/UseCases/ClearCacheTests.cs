using Core.Errors;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class ClearCacheTests
{
    private readonly ICache _cache = Substitute.For<ICache>();

    [Fact]
    public async Task HasAccess_NoException()
    {
        var result = await ExecuteAsync(true);
        
        result.Success.Should().BeTrue();
        _cache.Received().ClearAll();
    }
    
    [Fact]
    public async Task NoAccess_ReturnsError()
    {
        var result = await ExecuteAsync(false);
        
        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
        _cache.DidNotReceive().ClearAll();
    }

    private async Task<UseCaseResult<ClearCache.Result>> ExecuteAsync(bool canClearCache) => 
        await Sut.Execute(new ClearCache.Request(new AuthInTest(canClearCache: canClearCache)));

    private ClearCache Sut => new(_cache);
}