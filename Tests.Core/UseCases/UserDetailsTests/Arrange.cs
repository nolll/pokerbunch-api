using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Moq;
using NUnit.Framework;

namespace Tests.Core.UseCases.UserDetailsTests
{
    public class Arrange
    {
        private const int CurrentUserId = 1;
        private const int ViewUserId = 2;
        private string _currentUserName = "currentusername";
        protected const string ViewUserName = "viewusername";
        protected const string DisplayName = "displayname";
        protected const string RealName = "realname";
        protected const string Email = "email";
        protected virtual Role Role => Role.Player;
        protected virtual bool ViewingOwnUser => false; 
        protected UserDetails Sut;

        [SetUp]
        public void Setup()
        {
            var urm = new Mock<IUserRepository>();
            if (ViewingOwnUser)
            {
                urm.Setup(s => s.Get(ViewUserName)).Returns(new User(ViewUserId, ViewUserName, DisplayName, RealName, Email, Role));
                _currentUserName = ViewUserName;
            }
            else
            {
                urm.Setup(s => s.Get(_currentUserName)).Returns(new User(CurrentUserId, _currentUserName, globalRole: Role));
                urm.Setup(s => s.Get(ViewUserName)).Returns(new User(ViewUserId, ViewUserName, DisplayName, RealName, Email, Role));
            }

            Sut = new UserDetails(urm.Object);
        }

        protected UserDetails.Request Request => new UserDetails.Request(_currentUserName, ViewUserName);
    }
}
