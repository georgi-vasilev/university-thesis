namespace Domain.Common.Error
{
    using ErrorOr;

    public static class MoneyError
    {
        public static readonly Error InvalidAmountError = Error.Validation(
            code: "Money.Amount",
            description: "Invalid amount. Cannot be less than 0.");

        public static readonly Error InvalidCurrencyError = Error.Validation(
            code: "Money.Currency",
            description: "Currency cannot be null or empty.");

        public static readonly Error NullError = Error.Validation(
            code: "Money.Money",
            description: "Money value object cannot be null.");

    }
}
