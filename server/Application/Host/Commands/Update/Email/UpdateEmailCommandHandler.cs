namespace Application.Host.Commands.Update.Email
{
    using Domain.Event.Error;
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts.User;

    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, ErrorOr<UpdateEmailOutputModel>>
    {
        private readonly IHostDomainRepository _repository;
        private readonly ILogger<UpdateEmailCommandHandler> _logger;
        private readonly ICurrentUser _currentUser;

        public UpdateEmailCommandHandler(
            IHostDomainRepository repository,
            ILogger<UpdateEmailCommandHandler> logger,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<UpdateEmailOutputModel>> Handle(
            UpdateEmailCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUser.HostId is null)
            {
                _logger.LogWarning("HostId claim is missing for user {UserId}", _currentUser.UserId);
                return EventErrors.Unauthorized;
            }

            var hostId = _currentUser.HostId.Value;

            _logger.LogInformation("Handling UpdateEmailCommand for Host {HostId}", hostId);

            var host = await _repository.GetByIdAsync(hostId, cancellationToken);
            if (host is null) 
            {
                _logger.LogWarning("Host {HostId} not found.", hostId);
                return HostErrors.HostNotFoundError;
            }

            var updateResult = host.UpdateEmail(request.Email);
            if (updateResult.IsError)
            {
                _logger.LogWarning("UpdateEmail failed for Host {HostId}: {ErrorCode}", hostId, updateResult.FirstError.Code);
                return updateResult.FirstError;
            }

            try
            {
                await _repository.UpdateAsync(host, cancellationToken);
                _logger.LogInformation("Host {HostId} email updated to {Email} successfully.", hostId, request.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating email for Host {HostId}.", hostId);
                return HostErrors.UnexpectedError;
            }

            return new UpdateEmailOutputModel(host.Id, host.ContactInfo.Email);
        }
    }
}
