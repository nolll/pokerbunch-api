using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.GetBunchTests;

public abstract class Arrange : UseCaseTest<GetBunch>
{
    protected UseCaseResult<GetBunch.Result>? Result;

    private const string BunchId = "1";
    private const string UserId = "4";
    private const string PlayerId = "5";
    private const string Slug = "slug";
    private const string UserName = "username";
    private const string PlayerName = "playername";
    protected const string DisplayName = "displayname";
    protected const string Description = "description";
    protected const string HouseRules = "houserules";
    
    protected virtual bool CanGetBunch => false;
    protected virtual Role Role => Role.None;
        
    protected override void Setup()
    {
        var bunch = new Bunch(BunchId, Slug, DisplayName, Description, HouseRules);
        Mock<IBunchRepository>().Setup(s => s.GetBySlug(Slug)).Returns(Task.FromResult(bunch));
    }

    protected override async Task ExecuteAsync()
    {
        var currentBunch = new CurrentBunch(BunchId, Slug, DisplayName, PlayerId, PlayerName, Role);
        Result = await Sut.Execute(
            new GetBunch.Request(new AccessControlInTest(canGetBunch: CanGetBunch, currentBunch: currentBunch), Slug));
    }
}