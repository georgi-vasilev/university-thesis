namespace Domain.Common.Factory
{
    using Error;
    using ErrorOr;
    using ValueObject;

    internal static class MoneyFactory
    {
        public static ErrorOr<Money> Create(decimal amount, string currency)
        {
            if (amount < 0)
            {
                return MoneyError.InvalidAmountError;
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                return MoneyError.InvalidCurrencyError;
            }

            return new Money(amount, currency);
        }
    }
}
