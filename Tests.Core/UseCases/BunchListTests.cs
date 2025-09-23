using Core.Errors;
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
    public async Task BunchList_NoAccess_NoAccess()
    {
        var request = CreateRequest(canListBunches: false);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task BunchList_ReturnsListOfBunchItems()
    {
        var bunch1 = Create.Bunch();
        var bunch2 = Create.Bunch();
        _bunchRepository.List().Returns([bunch1, bunch2]);
        
        var request = CreateRequest();
        var result = await Sut.Execute(request);

        result.Data!.Bunches.Count.Should().Be(2);
        result.Data!.Bunches[0].Slug.Should().Be(bunch1.Slug);
        result.Data!.Bunches[0].Name.Should().Be(bunch1.DisplayName);
        result.Data!.Bunches[0].Description.Should().Be(bunch1.Description);
        result.Data!.Bunches[1].Slug.Should().Be(bunch2.Slug);
        result.Data!.Bunches[1].Name.Should().Be(bunch2.DisplayName);
        result.Data!.Bunches[1].Description.Should().Be(bunch2.Description);
    }

    private static GetBunchList.Request CreateRequest(bool? canListBunches = null) => 
        new(new AuthInTest(
            canListBunches: canListBunches ?? true));

    private GetBunchList Sut => new(_bunchRepository);
}