namespace Infrastructure.Services
{
    using ErrorOr;

    public static class PaymentError
    {
        public static Error PaymentProcessingError => Error.Failure(
            code: "Payment.ProcessingFailed",
            description: "Payment processing failed");

        public static Error UnexpectedPaymentError => Error.Unexpected(
            code: "Payment.UnexpectedError",
            description: "An unexpected error occurred during payment processing");

        public static Error PaymentNotFoundError => Error.NotFound(
            code: "Payment.NotFound",
            description: "Payment not found");

        public static Error InvalidPaymentDetailsError => Error.Validation(
            code: "Payment.InvalidDetails",
            description: "Invalid payment details provided");

        public static Error NegativeAmountError => Error.Validation(
            code: "Payment.NegativeAmount",
            description: "Payment amount cannot be negative");

        public static Error InvalidTransactionIdError => Error.Validation(
            code: "Payment.InvalidTransactionId",
            description: "Transaction ID is required and cannot be empty");

        public static Error PaymentStatusMismatchError => Error.Validation(
            code: "Payment.StatusMismatch",
            description: "Payment status does not match expected status");
    }
}
