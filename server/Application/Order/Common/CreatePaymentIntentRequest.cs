namespace Application.Order.Common
{
    public class CreatePaymentIntentRequest
    {
        public string EventId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int TicketQuantity { get; set; }
        public string TicketType { get; set; } = string.Empty;
    }
}
