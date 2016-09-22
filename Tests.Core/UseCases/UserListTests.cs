using System.Linq;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class UserListTests : TestBase
    {
        [Test]
        public void UserList_ReturnsListOfUserItems()
        {
            var result = Sut.Execute(new UserList.Request(TestData.AdminUser.UserName));

            Assert.AreEqual(4, result.Users.Count);
            Assert.AreEqual(TestData.UserDisplayNameA, result.Users.First().DisplayName);
            Assert.AreEqual("user-name-a", result.Users.First().UserName);
        }

        private UserList Sut
        {
            get
            {
                return new UserList(Services.UserService);
            }
        }
    }
}
