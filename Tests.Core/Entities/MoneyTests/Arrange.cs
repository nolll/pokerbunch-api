using Core.Entities;
using Tests.Common;

namespace Tests.Core.Entities.MoneyTests
{
    public abstract class Arrange : ArrangeBase
    {
        protected virtual Currency Currency => Currency.Default;
    }
}
