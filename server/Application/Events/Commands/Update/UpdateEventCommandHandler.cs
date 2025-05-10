namespace Application.Events.Commands.Update
{
    using Domain.Event;
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using Domain.Venue.Error;
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, ErrorOr<UpdateCommandOutputModel>>
    {
        private readonly IEventDomainRepository _eventRepository;
        private readonly IVenueDomainRepository _venueRepository;
        private readonly ILogger<UpdateEventCommandHandler> _logger;

        public UpdateEventCommandHandler(
            IEventDomainRepository eventRepository,
            IVenueDomainRepository venueRepository,
            ILogger<UpdateEventCommandHandler> logger)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<UpdateCommandOutputModel>> Handle(
            UpdateEventCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateEventCommand for Event {EventId}", request.EventId);
            var timeResult = TimeRange.FromDateTimes(request.StartTime, request.EndTime);

            if (timeResult.IsError)
            {
                return timeResult.FirstError;
            }

            var time = timeResult.Value;

            var eventToUpdate = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if(eventToUpdate is null)
            {
                _logger.LogWarning("Event {EventId} not found.", request.EventId);
                return EventErrors.EventNotFoundError;
            }

            var newVenue = await _venueRepository.GetByIdAsync(request.VenueId, cancellationToken);
            if (newVenue is null)
            {
                _logger.LogWarning("Venue {VenueId} not found.", request.VenueId);
                return VenueErrors.VenueNotFoundError;
            }

            var eventsAtVenue = await _eventRepository
                .GetEventsByFilterAsync(
                predicate: @event => @event.VenueId == request.VenueId && @event.Date == request.Date,
                cancellationToken);

            if (eventsAtVenue.Any(e => e.Time.OverlapsWith(time)))
            {
                _logger.LogWarning("Scheduling conflict detected for Venue {VenueId} on {Date}.", request.VenueId, request.Date);
                return EventErrors.OverlappingEventError;
            }


            var updateResult = eventToUpdate.UpdateDetails(
                request.Name,
                request.Description,
                request.Date,
                time,
                newVenue.Id);


            if (updateResult.IsError)
            {
                _logger.LogWarning(
                   "UpdateDetails failed for Event {EventId}: {ErrorCode}",
                   request.EventId,
                   updateResult.FirstError.Code);
                return updateResult.FirstError;
            }

            var updatedEvent = updateResult.Value;

            try
            {
                await _eventRepository.UpdateAsync(updatedEvent, cancellationToken);
                _logger.LogInformation("Event {EventId} updated successfully.", updatedEvent.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating Event {EventId}.",
                    updatedEvent.Id);
                return EventErrors.UnexpectedError;
            }

            return new UpdateCommandOutputModel(
                updatedEvent.Name,
                updatedEvent.Description,
                updatedEvent.Date,
                updatedEvent.Time,
                newVenue.Name);
        }
    }
}
