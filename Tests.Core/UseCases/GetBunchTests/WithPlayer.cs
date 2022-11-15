using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.GetBunchTests;

public class WithPlayer : Arrange
{
    protected override Role Role => Role.Player;

    [Test]
    public void BunchNameIsSet() => Assert.AreEqual(DisplayName, Result.Data.Name);

    [Test]
    public void DescriptionIsSet() => Assert.AreEqual(Description, Result.Data.Description);

    [Test]
    public void HouseRulesIsSet() => Assert.AreEqual(HouseRules, Result.Data.HouseRules);

    [Test]
    public void CanEditIsFalse() => Assert.AreEqual(Role.Player, Result.Data.Role);
}