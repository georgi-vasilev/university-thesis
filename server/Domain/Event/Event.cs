namespace Domain.Event
{
    using Domain.Event.Error;
    using ErrorOr;
    using Ticket;

    internal class Event
    {
        private readonly Guid _organizerId;
        private readonly HashSet<Ticket> _tickets = new HashSet<Ticket>();

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid OrganizerId { get; private set; }
        public DateOnly Date { get; private set; }
        public TimeRange Time { get; private set; }
        public Venue Venue { get; private set; }
        public EventStatus Status { get; private set; }

        internal Event(
            string name,
            DateOnly date,
            TimeRange time,
            Venue venue,
            Guid organizerId,
            Guid? id = null)
        {
            Name = name;
            Date = date;
            Time = time;
            Venue = venue;
            _organizerId = organizerId;
            Id = id ?? Guid.NewGuid();
            Status = EventStatus.Active;
        }

        public ErrorOr<Success> ChangeStatus(EventStatus newStatus)
        {
            if (Status == EventStatus.Cancelled && newStatus == EventStatus.Active)
            {
                return EventErrors.InvalidStatusChangeOperationFromCancelledToActive;
            }

            if (Status == EventStatus.Cancelled && newStatus == EventStatus.Postponed)
            {
                return EventErrors.InvalidStatusChangeOperationFromCancelledToPostponed;
            }

            Status = newStatus;

            return Result.Success;
        }
    }
}
