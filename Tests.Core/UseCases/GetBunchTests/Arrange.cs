using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using Moq;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases.GetBunchTests
{
    public abstract class Arrange
    {
        private const int BunchId = 1;
        private const int UserId = 4;
        private const int PlayerId = 5;
        private const string Slug = "slug";
        private const string UserName = "username";
        protected const string DisplayName = "displayname";
        protected const string Description = "description";
        protected const string HouseRules = "houserules";
        protected virtual Role Role => Role.None;
        private GetBunch.Request _request;
        private GetBunch _sut;

        [SetUp]
        public void Setup()
        {
            var bsm = new Mock<IBunchService>();
            bsm.Setup(s => s.GetBySlug(Slug)).Returns(new Bunch(BunchId, Slug, DisplayName, Description, HouseRules));

            var psm = new Mock<IPlayerService>();
            psm.Setup(s => s.GetByUserId(BunchId, UserId)).Returns(new Player(BunchId, PlayerId, UserId, role: Role));

            var urm = new Mock<IUserRepository>();
            urm.Setup(s => s.Get(UserName)).Returns(new User(UserId, UserName));

            _sut = new GetBunch(bsm.Object, urm.Object, psm.Object);
        }

        protected BunchResult Execute()
        {
            _request = new GetBunch.Request(UserName, Slug);
            return _sut.Execute(_request);
        }
    }
}
