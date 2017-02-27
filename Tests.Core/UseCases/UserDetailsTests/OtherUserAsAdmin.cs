using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.UserDetailsTests
{
    public class OtherUserAsAdmin : Arrange
    {
        protected override Role Role => Role.Admin;

        [Test]
        public void CanEditIsTrue() => Assert.IsTrue(Result.CanEdit);
    }
}
