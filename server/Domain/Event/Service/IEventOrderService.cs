namespace Domain.Event.Service
{
    using Common;
    using Common.ValueObject;
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
            Money price,
            TicketType type,
            CancellationToken cancellationToken);

        Task<ErrorOr<Success>> CancelTicketOrderAsync(
            Guid buyerId,
            Guid ticketId,
            CancellationToken cancellationToken);

        Task<ErrorOr<Success>> CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken);

        Task<ErrorOr<Success>> UpdateEventDetailsAsync(Host host, Event updatedEvent, CancellationToken cancellationToken);

        Task<ErrorOr<Success>> ChangeEventVenueAsync(Host host, Guid eventId, Guid newVenueId, CancellationToken cancellationToken);
    }
}
