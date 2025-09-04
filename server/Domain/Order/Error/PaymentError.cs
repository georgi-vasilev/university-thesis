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

        public static Error PaymentProcessingError => Error.Failure(
            code: "Payment.ProcessingFailed",
            description: "Payment processing failed");

        public static Error UnexpectedPaymentError => Error.Unexpected(
            code: "Payment.UnexpectedError",
            description: "An unexpected error occurred during payment processing");

        public static Error PaymentNotFoundError => Error.NotFound(
            code: "Payment.NotFound",
            description: "Payment not found");
    }
}
