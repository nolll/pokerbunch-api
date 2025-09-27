using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class AddCashgameTests : TestBase
{
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();
    
    [Fact]
    public async Task AddCashgame_WithLocation_GameIsAdded()
    {
        var locationId = Create.String();
        var bunch = Create.Bunch();
        _bunchRepository.GetBySlug(bunch.Slug).Returns(bunch);
        
        var request = CreateRequest(bunch.Slug, locationId);
        var result = await Sut.Execute(request);

        result.Data!.Slug.Should().Be(bunch.Slug);
        await _cashgameRepository.Received()
            .Add(
                Arg.Is<Bunch>(o => o.Id == bunch.Id),
                Arg.Is<Cashgame>(o => o.BunchSlug == bunch.Slug && o.LocationId == locationId));
    }

    [Fact]
    public async Task AddCashgame_WithoutLocation_ReturnsError()
    {
        var bunch = Create.Bunch();
        
        var request = CreateRequest(bunch.Slug);
        var result = await Sut.Execute(request);
        
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }
    
    private AddCashgame.Request CreateRequest(
        string slug,
        string? locationId = null)
    {
        return new AddCashgame.Request(new AuthInTest(canAddCashgame: true), slug, locationId);
    }

    private AddCashgame Sut => new(_bunchRepository, _cashgameRepository);
}