namespace Domain.Order
{
    using Common.ValueObject;
    using Error;
    using ErrorOr;
    using System;

    internal class Ticket
    {
        public Guid Id { get; private set; }
        public Guid EventId { get; private set; }
        public Money Price { get; private set; }
        public TicketStatus Status { get; private set; }
        public TicketType Type { get; private set; }
        public Guid? AttendeeId { get; private set; }

        internal Ticket(Guid eventId, Money price, TicketType type, Guid? id = null)
        {
            EventId = eventId;
            Price = price;
            Type = type;
            Status = TicketStatus.Available;
            Id = id ?? Guid.NewGuid();
        }

        public ErrorOr<Success> MarkAsSold(Guid attendeeId)
        {
            if (Status == TicketStatus.Sold)
            {
                return TicketError.AlreadySoldError;
            }

            AttendeeId = attendeeId;
            Status = TicketStatus.Sold;

            // TODO: dispatch domain event
            return Result.Success;
        }
    }
}
