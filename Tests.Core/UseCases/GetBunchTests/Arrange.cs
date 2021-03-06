using Core.Entities;
using Core.Repositories;
using Core.UseCases;

namespace Tests.Core.UseCases.GetBunchTests;

public abstract class Arrange : UseCaseTest<GetBunch>
{
    protected BunchResult Result;

    private const int BunchId = 1;
    private const int UserId = 4;
    private const int PlayerId = 5;
    private const string Slug = "slug";
    private const string UserName = "username";
    protected const string DisplayName = "displayname";
    protected const string Description = "description";
    protected const string HouseRules = "houserules";
    protected virtual Role Role => Role.None;
        
    protected override void Setup()
    {
        var bunch = new Bunch(BunchId, Slug, DisplayName, Description, HouseRules);
        var player = new Player(BunchId, PlayerId, UserId, UserName, role: Role);
        var user = new User(UserId, UserName);

        Mock<IBunchRepository>().Setup(s => s.GetBySlug(Slug)).Returns(bunch);
        Mock<IPlayerRepository>().Setup(s => s.Get(BunchId, UserId)).Returns(player);
        Mock<IUserRepository>().Setup(s => s.Get(UserName)).Returns(user);
    }

    protected override void Execute()
    {
        Result = Sut.Execute(new GetBunch.Request(UserName, Slug));
    }
}