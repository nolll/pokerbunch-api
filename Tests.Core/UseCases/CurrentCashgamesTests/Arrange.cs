using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.CurrentCashgamesTests;

public abstract class Arrange : UseCaseTest<CurrentCashgames>
{
    protected UseCaseResult<CurrentCashgames.Result>? Result;
    
    protected const string Slug = "default-slug";
    private const string BunchId = "2";
    protected const string CashgameId = "3";
    protected virtual int GameCount => 0;

    protected virtual bool CanListCurrentGames => false;

    protected override void Setup()
    {
        var cashgame = GameCount > 0 ? new CashgameInTest(id: CashgameId) : null;
        
        Mock<ICashgameRepository>().Setup(s => s.GetRunning(BunchId)).Returns(Task.FromResult<Cashgame?>(cashgame));
    }

    protected override async Task ExecuteAsync()
    {
        var currentBunch = new CurrentBunch(BunchId, Slug);
        Result = await Sut.Execute(new CurrentCashgames.Request(
            new PrincipalInTest(canListCurrentGames: CanListCurrentGames, currentBunch: currentBunch),
            Slug));
    }
}