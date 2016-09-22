using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;

namespace Tests.Core.UseCases.CashoutTests
{
    public class InvalidStack : Arrange
    {
        protected override int CashoutStack => -1;

        [Test]
        public void ThrowsValidationException()
        {
            var request = new Cashout.Request(UserName, Slug, PlayerId, CashoutStack, CashoutTime);

            Assert.Throws<ValidationException>(() => Sut.Execute(request));
        }
    }
}
