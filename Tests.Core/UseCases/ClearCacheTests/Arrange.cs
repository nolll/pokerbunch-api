using Core.Services;
using Core.UseCases;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ClearCacheTests;

public abstract class Arrange : UseCaseTest<ClearCache>
{
    protected UseCaseResult<ClearCache.Result>? Result;
    
    protected abstract bool CanClearCache { get; }

    protected override async Task ExecuteAsync()
    {
        Result = await Sut.Execute(new ClearCache.Request(new AuthInTest(canClearCache: CanClearCache)));
    }
}