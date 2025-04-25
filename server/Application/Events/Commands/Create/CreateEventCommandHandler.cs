namespace Application.Events.Commands.Create
{
    using Domain.Event.Builder;
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using Domain.Event.Service;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, ErrorOr<CreateEventOutputModel>>
    {
        private readonly IEventDomainRepository _repository;
        private readonly IEventBuilder _eventBuilder;
        private readonly IEventSchedulingService _eventScheduling;
        private readonly ILogger<CreateEventCommandHandler> _logger;

        public CreateEventCommandHandler(
            IEventDomainRepository repository,
            IEventBuilder eventBuilder,
            IEventSchedulingService eventScheduling,
            ILogger<CreateEventCommandHandler> logger)
        {
            _repository = repository;
            _eventBuilder = eventBuilder;
            _eventScheduling = eventScheduling;
            _logger = logger;
        }

        public async Task<ErrorOr<CreateEventOutputModel>> Handle(
            CreateEventCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Handling CreateEventCommand for Host {HostId}, Venue {VenueId}, Date {Date}",
                request.HostId, request.VenueId, request.Date);

            var schedulingResult = await _eventScheduling.ValidateNewEventAsync(request.VenueId, request.Date, request.Time, cancellationToken);
            if (schedulingResult.IsError)
            {
                _logger.LogWarning(
                    "Event scheduling validation failed for Venue {VenueId} on {Date}: {ErrorCode}",
                    request.VenueId, request.Date, schedulingResult.FirstError.Code);
                return schedulingResult.FirstError;
            }

            var eventBuildResult = _eventBuilder
                .WithName(request.Name)
                .WithDescription(request.Description)
                .WithDate(request.Date)
                .WithTime(request.Time)
                .WithHostId(request.HostId)
                .WithVenue(request.VenueId)
                .Build();

            if (eventBuildResult.IsError)
            {
                _logger.LogWarning(
                    "EventBuilder failed for Host {HostId}, Venue {VenueId}: {ErrorCode}",
                    request.HostId, request.VenueId, eventBuildResult.FirstError.Code);
                return eventBuildResult.FirstError;
            }

            var @event = eventBuildResult.Value;

            try
            {
                await _repository.AddAsync(@event, cancellationToken);
                _logger.LogInformation(
                    "Event {EventId} created successfully for Host {HostId}.",
                    @event.Id, request.HostId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error persisting Event for Host {HostId}, Venue {VenueId}.",
                    request.HostId, request.VenueId);
                return EventErrors.UnexpectedError;
            }

            return new CreateEventOutputModel(@event.Name, @event.Description, @event.Date, @event.Time);
        }
    }
}
