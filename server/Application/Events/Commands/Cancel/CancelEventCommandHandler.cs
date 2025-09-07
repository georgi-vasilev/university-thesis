namespace Application.Events.Commands.Cancel
{
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using Domain.Event.Service;
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts.User;
    using System.Threading;
    using System.Threading.Tasks;

    public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, ErrorOr<Success>>
    {
        private readonly IHostDomainRepository _hostRepository;
        private readonly ILogger<CancelEventCommandHandler> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IEventHostService _eventHostService;

        public CancelEventCommandHandler(
            IHostDomainRepository repository,
            ILogger<CancelEventCommandHandler> logger,
            ICurrentUser currentUser,
            IEventHostService eventHostService)
        {
            _hostRepository = repository;
            _logger = logger;
            _currentUser = currentUser;
            _eventHostService = eventHostService;
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
                "Handling CancelEventCommand for Host {HostId}, Event {Id}",
                hostId, request.Id);

            var host = await _hostRepository.GetByIdAsync(hostId, cancellationToken);


            if(host is null)
            {
                _logger.LogWarning("Host {HostId} not found.", hostId);
                return HostErrors.HostNotFoundError;
            }

            return await _eventHostService.CancelEventForHostAsync(host, request.Id, cancellationToken);
        }
    }
}
