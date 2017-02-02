using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.CurrentCashgamesTests
{
    public class WithGuest : Arrange
    {
        [Test]
        public void ReturnsListOfGames()
        {
            Assert.Throws<AccessDeniedException>(() => Sut.Execute(Request));
        }
    }
}