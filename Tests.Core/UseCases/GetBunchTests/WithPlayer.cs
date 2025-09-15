using Core.Entities;

namespace Tests.Core.UseCases.GetBunchTests;

public class WithPlayer : Arrange
{
    protected override bool CanGetBunch => true;
    protected override Role Role => Role.Player;

    [Test]
    public void BunchNameIsSet() => Result!.Data!.Name.Should().Be(DisplayName);

    [Test]
    public void DescriptionIsSet() => Result!.Data!.Description.Should().Be(Description);

    [Test]
    public void HouseRulesIsSet() => Result!.Data!.HouseRules.Should().Be(HouseRules);
}