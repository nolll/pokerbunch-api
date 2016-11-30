using System.Linq;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases
{
    public class InvitePlayerTests : TestBase
    {
        [Test]
        public void InvitePlayer_ReturnUrlIsSet()
        {
            var request = CreateRequest();
            var result = Sut.Execute(request);

            Assert.AreEqual(1, result.PlayerId);
        }

        [TestCase("")]
        [TestCase("a")]
        public void InvitePlayer_InvalidEmail_ThrowsException(string email)
        {
            var request = CreateRequest(email);

            var ex = Assert.Throws<ValidationException>(() => Sut.Execute(request));
            Assert.AreEqual(1, ex.Messages.Count());
        }

        [Test]
        public void InvitePlayer_ValidEmail_SendsInvitationEmail()
        {
            const string subject = "Invitation to Poker Bunch: Bunch A";
            const string body = @"You have been invited to join the poker game: Bunch A.

Use this link to accept the invitation: https://pokerbunch.com/fakejoin/bunch-a/d643c7857f8c3bffb1e9e7017a5448d09ef59d33. If the link doesn't work in your email client,
use this link instead, https://pokerbunch.com/fakejoin/bunch-a, and enter this verification code: d643c7857f8c3bffb1e9e7017a5448d09ef59d33

If you don't have an account, you can register at https://pokerbunch.com/test";
            var request = CreateRequest();

            Sut.Execute(request);

            Assert.AreEqual(TestData.UserEmailA, Deps.MessageSender.To);
            Assert.AreEqual(subject, Deps.MessageSender.Message.Subject);
            Assert.AreEqual(body, Deps.MessageSender.Message.Body);
        }

        private static InvitePlayer.Request CreateRequest(string email = TestData.UserEmailA)
        {
            return new InvitePlayer.Request(TestData.UserNameC, TestData.PlayerIdA, email, TestData.TestUrl, "https://pokerbunch.com/fakejoin/{0}", "https://pokerbunch.com/fakejoin/{0}/{1}");
        }

        private InvitePlayer Sut => new InvitePlayer(
            Deps.Bunch,
            Deps.Player,
            Deps.MessageSender,
            Deps.User);
    }
}
