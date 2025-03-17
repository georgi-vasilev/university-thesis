namespace Application.Events.Commands.Create
{
    using Domain.Event.Builder;
    using Domain.Event.Repository;
    using Domain.Event.Service;
    using ErrorOr;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateEventHandler : IRequestHandler<CreateEventCommand, ErrorOr<CreateEventOutputModel>>
    {
        private readonly IEventDomainRepository _repository;
        private readonly IEventBuilder _eventBuilder;
        private readonly IEventSchedulingService _eventScheduling;

        public CreateEventHandler(
            IEventDomainRepository repository,
            IEventBuilder eventBuilder,
            IEventSchedulingService eventScheduling)
        {
            _repository = repository;
            _eventBuilder = eventBuilder;
            _eventScheduling = eventScheduling;
        }

        public async Task<ErrorOr<CreateEventOutputModel>> Handle(
            CreateEventCommand request,
            CancellationToken cancellationToken)
        {
            var eventBuildResult = _eventBuilder
                .WithName(request.Name)
                .WithDescription(request.Description)
                .WithDate(request.Date)
                .WithTime(request.Time)
                .WithCapacity(request.Capacity)
                .WithHostId(request.HostId)
                .WithVenue(request.VenueId)
                .Build();

            var schedulingResult = await _eventScheduling.ValidateNewEventAsync(request.VenueId, request.Date, request.Time, cancellationToken);
            if (schedulingResult.IsError)
            {
                return schedulingResult.FirstError;
            }

            if (eventBuildResult.IsError)
            {
                return eventBuildResult.FirstError;
            }

            var @event = eventBuildResult.Value;
            await _repository.AddAsync(@event, cancellationToken);

            return new CreateEventOutputModel(@event.Name, @event.Description, @event.Date, @event.Time);
        }
    }
}
