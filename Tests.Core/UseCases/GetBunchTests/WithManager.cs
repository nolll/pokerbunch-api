using Core.Entities;
using NUnit.Framework;

namespace Tests.Core.UseCases.GetBunchTests
{
    public class WithManager : Arrange
    {
        protected override Role Role => Role.Manager;

        [Test]
        public void RoleIsManager()
        {
            var result = Sut.Execute(Request);
            Assert.AreEqual(Role.Manager, result.Role);
        }
    }
}