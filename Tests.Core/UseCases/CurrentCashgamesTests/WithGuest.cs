using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.CurrentCashgamesTests
{
    public class WithGuest : Arrange
    {
        protected override bool ExecuteAutomatically => false;

        [Test]
        public void ReturnsListOfGames() => Assert.Throws<AccessDeniedException>(Execute);
    }
}