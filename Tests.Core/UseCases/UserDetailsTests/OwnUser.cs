using NUnit.Framework;

namespace Tests.Core.UseCases.UserDetailsTests
{
    public class OwnUser : Arrange
    {
        protected override bool ViewingOwnUser => true;

        [Test]
        public void CanEditIsTrue()
        {
            Assert.IsTrue(Result.CanEdit);
        }

        [Test]
        public void CanChangePasswordIsTrue()
        {
            Assert.IsTrue(Result.CanChangePassword);
        }
    }
}
