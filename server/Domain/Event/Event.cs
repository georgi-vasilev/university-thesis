namespace Domain.Event
{
    using Common;
    using Error;
    using ErrorOr;

    public class Event : IAggregateRoot
    {
        private readonly HashSet<Guid> _ticketIds = new HashSet<Guid>();
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid HostId { get; private set; }
        public DateOnly Date { get; private set; }
        public TimeRange Time { get; private set; }
        public Guid VenueId { get; private set; }
        public EventStatus Status { get; private set; }
        public int TicketCount
        {
            get => this._ticketIds.Count;
        }

        internal Event(
            string name,
            string description,
            DateOnly date,
            TimeRange time,
            Guid venueId,
            Guid hostId,
            Guid? id = null)
        {
            Name = name;
            Description = description;
            Date = date;
            Time = time;
            VenueId = venueId;
            HostId = hostId;
            Id = id ?? Guid.NewGuid();
            Status = EventStatus.Active;
        }

        public ErrorOr<Success> AddTicket(Guid ticketId, int venueCapacity)
        {
            if (_ticketIds.Count >= venueCapacity)
            {
                return EventErrors.CapacityExceededError;
            }

            if (!_ticketIds.Add(ticketId))
            {
                return EventErrors.TicketAlreadyAddedError;
            }

            // TODO: Add logic to raise domain event
            // DomainEvents.Add(new TicketAddedEvent(Id, ticketId));

            return Result.Success;
        } 

        public ErrorOr<Success> ChangeStatus(EventStatus newStatus)
        {
            if (Status == EventStatus.Cancelled && newStatus == EventStatus.Active)
            {
                return EventErrors.InvalidStatusChangeOperationFromCancelledToActiveError;
            }

            if (Status == EventStatus.Cancelled && newStatus == EventStatus.Postponed)
            {
                return EventErrors.InvalidStatusChangeOperationFromCancelledToPostponedError;
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


        public ErrorOr<Event> UpdateDetails(string name,
            string description,
            DateOnly date,
            TimeRange time,
            Guid venueId)
        {
            if (string.IsNullOrEmpty(description))
            {
                return EventErrors.InvalidDescriptionError;
            }

            if (string.IsNullOrEmpty(name))
            {
                return EventErrors.InvalidNameError;
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

            return this;
        }

        private void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        private void ClearDomainEvents() => _domainEvents.Clear();
    }
}
