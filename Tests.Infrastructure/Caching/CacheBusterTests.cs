using Core.Services;
using Infrastructure.Data.Cache;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Infrastructure.Caching
{
    public class CacheBusterTests : TestBase
    {
        [Test]
        public void UserAdded_RemovesAllUserIds()
        {
            const string key = "UserIds:all";

            var sut = GetSut();
            sut.UserAdded();

            GetMock<ICacheContainer>().Verify(o => o.Remove(key));
        }

        [Test]
        public void UserUpdated_RemovesUserAndEmail()
        {
            const int userId = 1;
            const string userName = "a1";
            const string email = "a2";
            const string userKey = "User:1";
            const string nameKey = "UserId:nameoremail:a1";
            const string emailKey = "UserId:nameoremail:a2";
            var user = A.User.WithId(userId).WithUserName(userName).WithEmail(email).Build();

            var sut = GetSut();
            sut.UserUpdated(user);

            GetMock<ICacheContainer>().Verify(o => o.Remove(userKey));
            GetMock<ICacheContainer>().Verify(o => o.Remove(nameKey));
            GetMock<ICacheContainer>().Verify(o => o.Remove(emailKey));
        }

        private CacheBuster GetSut()
        {
            return new CacheBuster(GetMock<ICacheContainer>().Object);
        }
    }
}
