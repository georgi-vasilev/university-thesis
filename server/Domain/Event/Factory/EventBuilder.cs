namespace Domain.Event.Factory
{
    using Domain.Event.Error;
    using ErrorOr;

    internal class EventBuilder
    {
        private string _name;
        private string _description;
        private DateOnly _date;
        private TimeRange _time;
        private Guid _venueId;
        private Guid _hostId;
        private int _capacity;
        private Guid? _id;

        public EventBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public EventBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public EventBuilder WithDate(DateOnly date)
        {
            _date = date;
            return this;
        }

        public EventBuilder WithTime(TimeRange time)
        {
            _time = time;
            return this;
        }

        public EventBuilder WithVenue(Guid venueId)
        {
            _venueId = venueId;
            return this;
        }

        public EventBuilder WithHostId(Guid hostId)
        {
            _hostId = hostId;
            return this;
        }

        public EventBuilder WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public EventBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ErrorOr<Event> Build()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                return EventErrors.InvalidName;
            }

            if (string.IsNullOrWhiteSpace(_description))
            {
                return EventErrors.InvalidDescription;
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
                return EventErrors.InvalidCapacity;
            }

            if (_date < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return EventErrors.DateIsInThePastError;
            }

            return new Event(_name, _description, _date, _time, _venueId, _hostId, _capacity, _id);
        }
    }
}
