using Core.Exceptions;
using NUnit.Framework;

namespace Tests.Core.UseCases.CashoutTests
{
    public class InvalidStack : Arrange
    {
        protected override bool ExecuteAutomatically => false;
        protected override int CashoutStack => -1;

        [Test]
        public void ThrowsValidationException() => Assert.Throws<ValidationException>(Execute);
    }
}
