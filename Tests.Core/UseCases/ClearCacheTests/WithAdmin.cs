using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.ClearCacheTests
{
    public class WithAdmin : Arrange
    {
        protected override Role Role => Role.Admin;

        [Test]
        public void ThrowsException()
        {
            Assert.AreEqual(ExpectedNumberOfClearedObjects, Result.ClearCount);
        }
    }
}