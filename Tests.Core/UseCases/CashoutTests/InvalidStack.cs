using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.CashoutTests
{
    public class InvalidStack : Arrange
    {
        protected override int CashoutStack => -1;

        [Test]
        public void ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() => Sut.Execute(Request));
        }
    }
}
