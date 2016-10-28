using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases.GetBunchTests
{
    public abstract class Arrange : ArrangeBase
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
            _sut = CreateSut<GetBunch>();

            MockOf<IBunchService>().Setup(s => s.GetBySlug(Slug)).Returns(new Bunch(BunchId, Slug, DisplayName, Description, HouseRules));
            MockOf<IPlayerService>().Setup(s => s.GetByUserId(BunchId, UserId)).Returns(new Player(BunchId, PlayerId, UserId, role: Role));
            MockOf<IUserRepository>().Setup(s => s.Get(UserName)).Returns(new User(UserId, UserName));
        }

        protected BunchResult Execute()
        {
            _request = new GetBunch.Request(UserName, Slug);
            return _sut.Execute(_request);
        }
    }
}
