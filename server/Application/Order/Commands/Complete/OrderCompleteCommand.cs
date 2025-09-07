namespace Application.Order.Commands.Complete
{
    using ErrorOr;
    using MediatR;

    public record OrderCompleteCommand : IRequest<ErrorOr<OrderCompleteOutputModel>>
    {
        public OrderCompleteCommand(
            string transcationId,
            string paymentIntentStatus,
            string buyerId,
            string eventId,
            long expectedAmount)
        {
            TranscationId = transcationId;
            PaymentIntentStatus = paymentIntentStatus;
            Amount = expectedAmount;
            BuyerId = buyerId;
            EventId = eventId;
        }

        public string TranscationId { get; init; }
        public string PaymentIntentStatus { get; init; }
        public string BuyerId { get; init; }
        public string EventId { get; init; }
        public long Amount{ get; init; }
    }
}
