using Core.Services;
using Core.UseCases;

namespace Tests.Core.UseCases.ClearCacheTests;

public abstract class Arrange : UseCaseTest<ClearCache>
{
    protected UseCaseResult<ClearCache.Result>? Result;
    
    protected abstract IAccessControl AccessControl { get; }

    protected override async Task ExecuteAsync()
    {
        Result = await Sut.Execute(new ClearCache.Request(AccessControl));
    }
}