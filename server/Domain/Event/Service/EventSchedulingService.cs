namespace Domain.Event.Service
{
    using Error;
    using ErrorOr;
    using Repository;

    internal class EventSchedulingService : IEventSchedulingService
    {
        private readonly IEventDomainRepository _eventRepository;

        public EventSchedulingService(IEventDomainRepository eventRepository) 
            => _eventRepository = eventRepository;

        public async Task<ErrorOr<Success>> ValidateNewEventAsync(
            Guid venueId,
            DateOnly date,
            TimeRange newEventTime,
            CancellationToken cancellationToken)
        {
            var eventsAtVenue = await _eventRepository
                .GetEventsByFilterAsync(
                predicate: @event => @event.VenueId == venueId && @event.Date == date,
                cancellationToken);

            if (eventsAtVenue.Any(e => e.Time.OverlapsWith(newEventTime)))
            {
                return EventErrors.OverlappingEventError;
            }

            return Result.Success;
        }
    }

}
