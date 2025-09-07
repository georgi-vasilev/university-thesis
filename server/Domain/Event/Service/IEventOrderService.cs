namespace Domain.Event.Service
{
    using Common;
    using ErrorOr;
    using Host;
    using Order;
    using System;
    using System.Threading.Tasks;

    public interface IEventOrderService : IDomainService
    {
        Task<ErrorOr<Success>> PurchaseTicketAsync(
            Guid buyerId,
            Guid eventId,
            string transactionId,
            int quantity,
            TicketType type,
            CancellationToken cancellationToken);

        Task<ErrorOr<Success>> CancelTicketOrderAsync(
            Guid buyerId,
            Guid ticketId,
            CancellationToken cancellationToken);

        Task<ErrorOr<Order>> CompleteOrderAsync(
            Guid orderId,
            Guid buyerId,
            string transactionId,
            string paymentIntentStatus,
            long amount,
            CancellationToken cancellationToken);

        Task<ErrorOr<Success>> UpdateEventDetailsAsync(Host host, Event updatedEvent, CancellationToken cancellationToken);

        Task<ErrorOr<Success>> ChangeEventVenueAsync(Host host, Guid eventId, Guid newVenueId, CancellationToken cancellationToken);
    }
}
