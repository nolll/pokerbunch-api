using Core.Entities;

namespace Tests.Core.Entities.MoneyTests;

public abstract class Arrange
{
    protected virtual Currency Currency => Currency.Default;
}