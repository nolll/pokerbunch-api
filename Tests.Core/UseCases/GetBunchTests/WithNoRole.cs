using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.GetBunchTests
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
