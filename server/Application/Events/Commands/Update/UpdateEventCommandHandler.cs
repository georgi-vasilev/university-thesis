namespace Application.Events.Commands.Update
{
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using Domain.Venue.Error;
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;

    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, ErrorOr<UpdateCommandOutputModel>>
    {
        private readonly IEventDomainRepository _eventRepository;
        private readonly IVenueDomainRepository _venueRepository;

        public UpdateEventCommandHandler(IEventDomainRepository eventRepository, IVenueDomainRepository venueRepository)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
        }

        public async Task<ErrorOr<UpdateCommandOutputModel>> Handle(
            UpdateEventCommand request,
            CancellationToken cancellationToken)
        {
            var eventToUpdate = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if(eventToUpdate is null)
            {
                return EventErrors.EventNotFoundError;
            }

            var newVenue = await _venueRepository.GetByIdAsync(request.VenueId, cancellationToken);
            if (newVenue is null)
            {
                return VenueErrors.VenueNotFoundError;
            }

            var eventsAtVenue = await _eventRepository
                .GetEventsByFilter(
                predicate: @event => @event.VenueId == request.VenueId && @event.Date == request.Date,
                cancellationToken);

            if (eventsAtVenue.Any(e => e.Time.OverlapsWith(request.Time)))
            {
                return EventErrors.OverlappingEventError;
            }


            var updateResult = eventToUpdate.UpdateDetails(
                request.Name,
                request.Description,
                request.Date,
                request.Time,
                newVenue.Id);


            if (updateResult.IsError)
            {
                return updateResult.FirstError;
            }

            var updatedEvent = updateResult.Value;
            await _eventRepository.UpdateAsync(updatedEvent, cancellationToken);

            return new UpdateCommandOutputModel(
                updatedEvent.Name,
                updatedEvent.Description,
                updatedEvent.Date,
                updatedEvent.Time,
                newVenue.Name);
        }
    }
}
