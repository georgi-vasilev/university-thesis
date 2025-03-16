namespace Domain.Order.Builder
{
    using Common.ValueObject;
    using ErrorOr;

    public interface ITicketBuilder
    {
        ITicketBuilder WithEventId(Guid eventId);
        ITicketBuilder WithPrice(Money price);
        ITicketBuilder WithType(TicketType type);
        ErrorOr<Ticket> Build();
    }
}
