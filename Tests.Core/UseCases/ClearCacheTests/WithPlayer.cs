using Core.Entities;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.ClearCacheTests
{
    public class WithPlayer : Arrange
    {
        protected override bool ExecuteAutomatically => false;
        protected override Role Role => Role.Player;

        [Test]
        public void ThrowsException()
        {
            Assert.Throws<AccessDeniedException>(Execute);
        }
    }
}