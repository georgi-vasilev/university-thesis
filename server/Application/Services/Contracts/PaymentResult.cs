namespace Application.Services.Contracts
{
    public record PaymentResult
    {
        public PaymentResult(string transactionId, PaymentStatus status)
        {
            TransactionId = transactionId;
            Status = status;
        }

        public string TransactionId { get; init; }
        public PaymentStatus Status { get; init; }
    }
}
