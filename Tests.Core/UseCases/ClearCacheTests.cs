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
        var request = CreateRequest();
        var result = await Sut.Execute(request);
        
        result.Success.Should().BeTrue();
        _cache.Received().ClearAll();
    }
    
    [Fact]
    public async Task NoAccess_ReturnsError()
    {
        var request = CreateRequest(false);
        var result = await Sut.Execute(request);
        
        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
        _cache.DidNotReceive().ClearAll();
    }

    private ClearCache.Request CreateRequest(bool? canClearCache = null) => 
        new(new AuthInTest(
            canClearCache: canClearCache ?? true));

    private ClearCache Sut => new(_cache);
}