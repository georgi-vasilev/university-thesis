namespace Domain.Order
{
    public record PaymentDetails
    {
        public decimal Amount { get; }
        public string PaymentMethod { get; }
        public PaymentStatus Status { get; }
        public string TransactionId { get; }

        public PaymentDetails(decimal amount, string paymentMethod, PaymentStatus status, string transactionId)
        {
            Amount = amount;
            PaymentMethod = paymentMethod;
            Status = status;
            TransactionId = transactionId;

        }

        public override string ToString() => $"{PaymentMethod}: {Amount:C} ({Status})";
    }
}
