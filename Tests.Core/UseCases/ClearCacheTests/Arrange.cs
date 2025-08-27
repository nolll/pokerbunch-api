using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.ClearCacheTests;

public abstract class Arrange : UseCaseTest<ClearCache>
{
    protected UseCaseResult<ClearCache.Result>? Result;
    
    protected abstract bool IsAdmin { get; }

    protected override async Task ExecuteAsync()
    {
        var currentUser = new CurrentUser("", "", "", IsAdmin);
        Result = await Sut.Execute(new ClearCache.Request(currentUser));
    }
}