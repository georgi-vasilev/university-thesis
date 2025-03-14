namespace Domain.Event
{
    using Domain.Common;
    using Domain.Event.Error;
    using ErrorOr;

    internal class Event
    {
        private readonly Guid _hostId;
        private readonly HashSet<Guid> _ticketIds = new HashSet<Guid>();
        public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid HostId { get; private set; }
        public DateOnly Date { get; private set; }
        public TimeRange Time { get; private set; }
        public Guid VenueId { get; private set; }
        public EventStatus Status { get; private set; }
        public int Capacity { get; private set; }

        internal Event(
            string name,
            DateOnly date,
            TimeRange time,
            Guid venueId,
            Guid hostId,
            int capacity,
            Guid? id = null)
        {
            Name = name;
            Date = date;
            Time = time;
            VenueId = venueId;
            _hostId = hostId;
            Capacity = capacity;
            Id = id ?? Guid.NewGuid();
            Status = EventStatus.Active;
        }

        public ErrorOr<Success> AddTicket(Guid ticketId)
        {
            if (_ticketIds.Count >= Capacity)
            {
                return EventErrors.CapacityExceeded;
            }

            if (!_ticketIds.Add(ticketId))
            {
                return EventErrors.TicketAlreadyAdded;
            }

            // TODO: Add logic to raise domain event
            // DomainEvents.Add(new TicketAddedEvent(Id, ticketId));

            return Result.Success;
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

        public ErrorOr<Success> Reschedule(DateTime newStart, DateTime newEnd)
        {
            DateOnly newDate = DateOnly.FromDateTime(newStart);

            var newTimeRangeResult = TimeRange.FromDateTimes(newStart, newEnd);
            if (newTimeRangeResult.IsError)
            {
                return newTimeRangeResult.FirstError;
            }

            if (newDate < DateOnly.FromDateTime(DateTime.Now))
            {
                return EventErrors.DateIsInThePastError;
            }

            Date = newDate;
            Time = newTimeRangeResult.Value;

            // TODO: Add logic to raise domain event
            // DomainEvents.Add(new EventRescheduledEvent(Id, Date, Time));

            return Result.Success;
        }


        public ErrorOr<Success> UpdateDetails(string name,
            string description,
            DateOnly date,
            TimeRange time,
            Guid venueId)
        {
            if (string.IsNullOrEmpty(description))
            {
                return EventErrors.InvalidDescription;
            }

            if (string.IsNullOrEmpty(name))
            {
                return EventErrors.InvalidName;
            }

            if (date < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return EventErrors.DateIsInThePastError;
            }

            if (venueId == Guid.Empty)
            {
                return EventErrors.NullVenueError;
            }

            Name = name;
            Description = description;
            Date = date;
            Time = time;
            VenueId = venueId;

            // TODO: Add logic to raise domain event
            // DomainEvents.Add(new EventDetailsUpdatedEvent(Id, name, description, date, time, venueId));

            return Result.Success;
        }
    }
}
