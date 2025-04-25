namespace Application.Events.Commands.Cancel
{
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, ErrorOr<Success>>
    {
        private readonly IHostDomainRepository _hostRepository;
        private readonly IEventDomainRepository _eventRepository;
        private readonly ILogger<CancelEventCommandHandler> _logger;

        public CancelEventCommandHandler(
            IHostDomainRepository repository,
            IEventDomainRepository eventRepository,
            ILogger<CancelEventCommandHandler> logger)
        {
            _hostRepository = repository;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<Success>> Handle(
            CancelEventCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Handling CancelEventCommand for Host {HostId}, Event {EventId}, NewStatus {Status}",
                request.HostId, request.EventId, request.Status);

            var host = await _hostRepository.GetByIdAsync(request.HostId, cancellationToken);

            if(host is null)
            {
                _logger.LogWarning("Host {HostId} not found.", request.HostId);
                return HostErrors.HostNotFoundError;
            }

            if (!host.OrganizedEventIds.Any(id => id == request.EventId))
            {
                _logger.LogWarning(
                    "Host {HostId} does not organize Event {EventId}.",
                    request.HostId, request.EventId);
                return HostErrors.EventDoesNotExistError;
            }

            var @event = await _eventRepository.GetEventByFilterAsync(
                predicate: e => e.HostId == request.HostId && e.Id == request.EventId,
                cancellationToken);

            if (@event is null)
            {
                _logger.LogWarning(
                    "Event {EventId} not found for Host {HostId}.",
                    request.EventId, request.HostId);
                return EventErrors.EventNotFoundError;
            }

            var changeStatusResult = @event.ChangeStatus(request.Status);

            if (changeStatusResult.IsError)
            {
                _logger.LogWarning(
                   "Failed to change status of Event {EventId} to {Status}: {ErrorCode}",
                   request.EventId,
                   request.Status,
                   changeStatusResult.FirstError.Code);
                return changeStatusResult.FirstError;
            }

            await _eventRepository.UpdateAsync(@event, cancellationToken);

            _logger.LogInformation(
             "Event {EventId} status successfully changed to {Status}.",
             request.EventId, request.Status);

            return Result.Success;
        }
    }
}
