using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.BunchDetailsTests
{
    public class WithNoRole : Arrange
    {
        [Test]
        public void AccessDenied()
        {
            Assert.Throws<AccessDeniedException>(() => Execute());
        }
    }
}
