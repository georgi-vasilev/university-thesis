namespace Domain.Order
{
    using Common.ValueObject;
    using Error;
    using ErrorOr;
    using System;

    public class Ticket
    {
        public Guid Id { get; private set; }
        public Guid EventId { get; private set; }
        public Money Price { get; private set; }
        public bool HasBeenUsed { get; private set; }
        public TicketStatus Status { get; private set; }
        public TicketType Type { get; private set; }
        public Guid? AttendeeId { get; private set; }

        private Ticket()
        {
            
        }

        internal Ticket(Guid eventId, Money price, TicketType type, Guid? id = null)
        {
            EventId = eventId;
            Price = price;
            Type = type;
            Status = TicketStatus.Available;
            HasBeenUsed = false;
            Id = id ?? Guid.NewGuid();
        }

        public ErrorOr<Success> AddToBuyer(Guid attendeeId)
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
