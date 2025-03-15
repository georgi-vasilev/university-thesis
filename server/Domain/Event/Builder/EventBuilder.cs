namespace Domain.Event.Factory
{
    using Domain.Event.Builder;
    using Error;
    using ErrorOr;

    internal class EventBuilder : IEventBuilder
    {
        private string _name = default!;
        private string _description = default!;
        private DateOnly _date;
        private TimeRange _time = default!;
        private Guid _venueId;
        private Guid _hostId;
        private int _capacity;
        private Guid? _id;

        public IEventBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public IEventBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public IEventBuilder WithDate(DateOnly date)
        {
            _date = date;
            return this;
        }

        public IEventBuilder WithTime(TimeRange time)
        {
            _time = time;
            return this;
        }

        public IEventBuilder WithVenue(Guid venueId)
        {
            _venueId = venueId;
            return this;
        }

        public IEventBuilder WithHostId(Guid hostId)
        {
            _hostId = hostId;
            return this;
        }

        public IEventBuilder WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public IEventBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ErrorOr<Event> Build()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                return EventErrors.InvalidNameError;
            }

            if (string.IsNullOrWhiteSpace(_description))
            {
                return EventErrors.InvalidDescriptionError;
            }

            if (_date == default)
            {
                return EventErrors.DefaultDateValueError;
            }

            if (_time is null)
            {
                return EventErrors.NullTimeError;
            }

            if (_time.End <= _time.Start)
            {
                return EventErrors.InvalidEndTimeError;
            }

            if (_venueId == Guid.Empty)
            {
                return EventErrors.NullVenueError;
            }

            if (_hostId == default)
            {
                return EventErrors.InvalidOrganizerIdValueError;
            }

            if (_capacity <= 0)
            {
                return EventErrors.InvalidCapacityError;
            }

            if (_date < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return EventErrors.DateIsInThePastError;
            }

            return new Event(_name, _description, _date, _time, _venueId, _hostId, _capacity, _id);
        }
    }
}
