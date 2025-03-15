namespace Domain.Order.Error
{
    using ErrorOr;

    internal static class PaymentError
    {
        public static readonly Error InvalidPaymentDetailsError = Error.Validation(
          code: "Payment.Update",
          description: "Invalid payment details.");

        public static readonly Error NegativeAmountError = Error.Validation(
            code: "Payment.Complete",
            description: "Payment amount cannot be negative.");

        public static readonly Error InvalidTransactionIdError = Error.Validation(
            code: "Payment.Complete",
            description: "Transcation id cannot be null or smpty string.");

        public static readonly Error PaymentStatusMismatchError = Error.Validation(
            code: "Payment.Complete",
            description: "Payment status is not completed!.");
    }
}
