namespace Tests.Core.UseCases.UserDetailsTests;

public class OwnUser : Arrange
{
    protected override bool ViewingOwnUser => true;

    [Test]
    public void CanViewAllIsTrue() => Result!.Data!.CanViewAll.Should().BeTrue();
}