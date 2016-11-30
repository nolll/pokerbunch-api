using System.Linq;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    class AddPlayerTests : TestBase
    {
        private const string EmptyName = "";
        private const string UniqueName = "Unique Name";
        private const string ExistingName = TestData.PlayerNameA;

        [Test]
        public void AddPlayer_ReturnUrlIsSet()
        {
            var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, UniqueName);
            var result = Sut.Execute(request);

            Assert.AreEqual("bunch-a", result.Slug);
        }

        [Test]
        public void AddPlayer_EmptyName_ThrowsException()
        {
            var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, EmptyName);

            var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
            Assert.AreEqual(1, ex.Messages.Count());
        }

        [Test]
        public void AddPlayer_ValidName_AddsPlayer()
        {
            var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, UniqueName);
            Sut.Execute(request);

            Assert.IsNotNull(Deps.Player.Added);
        }

        [Test]
        public void AddPlayer_ValidNameButNameExists_ThrowsException()
        {
            var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, ExistingName);
            Assert.Throws<PlayerExistsException>(() => Sut.Execute(request));
        }

        private AddPlayer Sut => new AddPlayer(
            Deps.Bunch,
            Deps.Player,
            Deps.User);
    }
}
