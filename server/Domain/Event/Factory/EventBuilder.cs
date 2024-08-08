namespace Domain.Event.Factory
{
    using Domain.Event.Error;
    using ErrorOr;

    internal class EventBuilder
    {
        private string _name;
        private DateOnly _date;
        private TimeRange _time;
        private Venue _venue;
        private Guid _organizerId;
        private Guid? _id;

        public EventBuilder WithName(string name)
        {
            _name = name;
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

        public EventBuilder WithVenue(Venue venue)
        {
            _venue = venue;
            return this;
        }

        public EventBuilder WithOrganizerId(Guid organizerId)
        {
            _organizerId = organizerId;
            return this;
        }

        public EventBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ErrorOr<Event> Build()
        {
            if (_date == default)
            {
                return EventErrors.DefaultDateValueError;
            }

            if (_time is null)
            {
                return EventErrors.NullTimeError;
            }

            if (_venue is null)
            {
                return EventErrors.NullVenueError;
            }

            if (_organizerId == default)
            {
                return EventErrors.InvalidOrganizerIdValueError;
            }

            if (_time.End <= _time.Start)
            {
                return EventErrors.InvalidEndTimeError;
            }

            return new Event(_name, _date, _time, _venue, _organizerId, _id);
        }
    }

}
