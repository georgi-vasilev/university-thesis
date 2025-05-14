namespace Application.Events.Commands.Cancel
{
    using Application.Services.Contracts.User;
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
        private readonly ICurrentUser _currentUser;

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
            if (_currentUser.HostId is null)
            {
                _logger.LogWarning("HostId claim is missing for user {UserId}", _currentUser.UserId);
                return EventErrors.Unauthorized;
            }

            var hostId = _currentUser.HostId.Value;

            _logger.LogInformation(
                "Handling CancelEventCommand for Host {HostId}, Event {EventId}, NewStatus {Status}",
                hostId, request.EventId, request.Status);

            var host = await _hostRepository.GetByIdAsync(hostId, cancellationToken);

            if(host is null)
            {
                _logger.LogWarning("Host {HostId} not found.", hostId);
                return HostErrors.HostNotFoundError;
            }

            if (!host.OrganizedEventIds.Any(id => id == request.EventId))
            {
                _logger.LogWarning(
                    "Host {HostId} does not organize Event {EventId}.",
                    hostId, request.EventId);
                return HostErrors.EventDoesNotExistError;
            }

            var @event = await _eventRepository.GetEventByFilterAsync(
                predicate: e => e.HostId == hostId && e.Id == request.EventId,
                cancellationToken);

            if (@event is null)
            {
                _logger.LogWarning(
                    "Event {EventId} not found for Host {HostId}.",
                    request.EventId, hostId);
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
