using NUnit.Framework;

namespace Tests.Core.UseCases.UserDetailsTests
{
    public class OwnUser : Arrange
    {
        protected override bool ViewingOwnUser => true;

        [Test]
        public void CanEditIsTrue()
        {
            var result = Sut.Execute(Request);
            Assert.IsTrue(result.CanEdit);
        }

        [Test]
        public void CanChangePasswordIsTrue()
        {
            var result = Sut.Execute(Request);
            Assert.IsTrue(result.CanChangePassword);
        }
    }
}
