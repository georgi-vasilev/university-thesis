namespace Domain.Event.Service
{
    using Common.ValueObject;
    using ErrorOr;
    using Host;
    using Order;
    using System;
    using System.Threading.Tasks;

    public interface IEventOrderService
    {
        Task<ErrorOr<Success>> PurchaseTicketAsync(
            Guid buyerId,
            Guid eventId,
            Money price,
            TicketType type);

        Task<ErrorOr<Success>> CancelTicketOrderAsync(
            Guid buyerId,
            Guid ticketId);

        Task<ErrorOr<Success>> CompleteOrderAsync(Guid orderId);

        Task<ErrorOr<Success>> UpdateEventDetailsAsync(Host host, Event updatedEvent);

        Task<ErrorOr<Success>> ChangeEventVenueAsync(Host host, Guid eventId, Guid newVenueId);
    }
}
