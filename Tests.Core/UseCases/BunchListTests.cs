using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class BunchListTests : TestBase
{
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>(); 
    
    [Fact]
    public async Task BunchList_ReturnsListOfBunchItems()
    {
        var bunch1 = Create.Bunch();
        var bunch2 = Create.Bunch();
        _bunchRepository.List().Returns([bunch1, bunch2]);
        
        var result = await Sut.Execute(new GetBunchList.Request(new AuthInTest(canListBunches: true)));

        result.Data!.Bunches.Count.Should().Be(2);
        result.Data!.Bunches[0].Slug.Should().Be(bunch1.Slug);
        result.Data!.Bunches[0].Name.Should().Be(bunch1.DisplayName);
        result.Data!.Bunches[0].Description.Should().Be(bunch1.Description);
        result.Data!.Bunches[1].Slug.Should().Be(bunch2.Slug);
        result.Data!.Bunches[1].Name.Should().Be(bunch2.DisplayName);
        result.Data!.Bunches[1].Description.Should().Be(bunch2.Description);
    }

    private GetBunchList Sut => new(_bunchRepository);
}