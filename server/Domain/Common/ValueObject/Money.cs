namespace Domain.Common.ValueObject
{
    using System;

    internal record Money : IEquatable<Money>
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public override string ToString() 
            => $"{Currency} {Amount:N2}";
    }
}
