using System;

namespace Core.Entities
{
    public class Money : IComparable<Money>
    {
        public int Amount { get; }
        private readonly Currency _currency;
        
        public Money(int amount, Currency currency = null)
        {
            Amount = amount;
            _currency = currency ?? Currency.Default;
        }

        public int CompareTo(Money other)
        {
            return Amount.CompareTo(other.Amount);
        }

        public override string ToString()
        {
            var absAmount = Math.Abs(Amount);
            var numberFormatted = absAmount.ToString("N0", _currency.Culture);
            var numberWithSymbol = string.Format(_currency.Format, numberFormatted);
            return Amount < 0 ? $"-{numberWithSymbol}" : numberWithSymbol;
        }
    }
}