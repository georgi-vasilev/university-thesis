namespace Application.Events.Commands.Create
{
    using Domain.Event;
    using Domain.Event.Builder;
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using Domain.Event.Service;
    using Domain.Venue.Error;
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts.User;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, ErrorOr<CreateEventOutputModel>>
    {
        private readonly IEventDomainRepository _repository;
        private readonly IEventBuilder _eventBuilder;
        private readonly IEventSchedulingService _eventScheduling;
        private readonly ILogger<CreateEventCommandHandler> _logger;
        private readonly IVenueDomainRepository _venueRepository;
        private readonly ICurrentUser _currentUser;

        public CreateEventCommandHandler(
            IEventDomainRepository repository,
            IEventBuilder eventBuilder,
            IEventSchedulingService eventScheduling,
            ILogger<CreateEventCommandHandler> logger,
            IVenueDomainRepository venueRepository,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _eventBuilder = eventBuilder;
            _eventScheduling = eventScheduling;
            _logger = logger;
            _venueRepository = venueRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<CreateEventOutputModel>> Handle(
            CreateEventCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUser.HostId is null)
            {
                _logger.LogWarning("HostId claim is missing for user {UserId}", _currentUser.UserId);
                return EventErrors.Unauthorized;
            }

            var hostId = _currentUser.HostId.Value;

            _logger.LogInformation(
                "Handling CreateEventCommand for Host {HostId}, Venue {VenueId}, Date {Date}",
                hostId, request.VenueId, request.Date);

            var time = TimeRange.FromDateTimes(request.StartTime, request.EndTime).Value;

            var schedulingResult = await _eventScheduling.ValidateNewEventAsync(request.VenueId, request.Date, time, cancellationToken);
            if (schedulingResult.IsError)
            {
                _logger.LogWarning(
                    "Event scheduling validation failed for Venue {VenueId} on {Date}: {ErrorCode}",
                    request.VenueId, request.Date, schedulingResult.FirstError.Code);
                return schedulingResult.FirstError;
            }

            var venue = await _venueRepository.GetByIdAsync(request.VenueId, cancellationToken);
            if(venue is null)
            {
                return VenueErrors.VenueNotFoundError;
            }

            if(venue.Capacity < request.Capacity)
            {
                return VenueErrors.CapacityExceededError;
            }

            var eventBuildResult = _eventBuilder
                .WithName(request.Name)
                .WithDescription(request.Description)
                .WithDate(request.Date)
                .WithTime(time)
                .WithHostId(hostId)
                .WithVenue(request.VenueId)
                .Build();

            if (eventBuildResult.IsError)
            {
                _logger.LogWarning(
                    "EventBuilder failed for Host {HostId}, Venue {VenueId}: {ErrorCode}",
                    hostId, request.VenueId, eventBuildResult.FirstError.Code);
                return eventBuildResult.FirstError;
            }   

            var @event = eventBuildResult.Value;

            try
            {
                await _repository.AddAsync(@event, cancellationToken);
                _logger.LogInformation(
                    "Event {EventId} created successfully for Host {HostId}.",
                    @event.Id, hostId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error persisting Event for Host {HostId}, Venue {VenueId}.",
                    hostId, request.VenueId);
                return EventErrors.UnexpectedError;
            }

            return new CreateEventOutputModel(@event.Name, @event.Description, @event.Date, @event.Time);
        }
    }
}
