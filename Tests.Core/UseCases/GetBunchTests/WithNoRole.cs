using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.GetBunchTests
{
    public class WithNoRole : Arrange
    {
        protected override bool ExecuteAutomatically => false;

        [Test]
        public void AccessDenied()
        {
            Assert.Throws<AccessDeniedException>(Execute);
        }
    }
}
