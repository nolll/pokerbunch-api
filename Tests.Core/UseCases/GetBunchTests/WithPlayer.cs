using Core.Entities;

namespace Tests.Core.UseCases.GetBunchTests;

public class WithPlayer : Arrange
{
    protected override bool CanGetBunch => true;
    protected override Role Role => Role.Player;

    [Test]
    public void BunchNameIsSet() => Assert.That(Result?.Data?.Name, Is.EqualTo(DisplayName));

    [Test]
    public void DescriptionIsSet() => Assert.That(Result?.Data?.Description, Is.EqualTo(Description));

    [Test]
    public void HouseRulesIsSet() => Assert.That(Result?.Data?.HouseRules, Is.EqualTo(HouseRules));
}