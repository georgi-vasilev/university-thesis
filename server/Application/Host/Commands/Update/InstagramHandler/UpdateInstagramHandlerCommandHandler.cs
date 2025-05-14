namespace Application.Host.Commands.Update.InstagramHandler
{
    using Domain.Event.Error;
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts.User;

    public class UpdateInstagramHandlerCommandHandler : IRequestHandler<UpdateInstagramHandlerCommand, ErrorOr<UpdateInstagramHandlerOutputModel>>
    {
        private readonly IHostDomainRepository _repository;
        private readonly ILogger<UpdateInstagramHandlerCommandHandler> _logger;
        private readonly ICurrentUser _currentUser;

        public UpdateInstagramHandlerCommandHandler(
            IHostDomainRepository repository,
            ILogger<UpdateInstagramHandlerCommandHandler> logger,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<UpdateInstagramHandlerOutputModel>> Handle(
            UpdateInstagramHandlerCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUser.HostId is null)
            {
                _logger.LogWarning("HostId claim is missing for user {UserId}", _currentUser.UserId);
                return EventErrors.Unauthorized;
            }

            var hostId = _currentUser.HostId.Value;

            _logger.LogInformation("Handling UpdateInstagramCommand for Host {HostId}", hostId);
            var host = await _repository.GetByIdAsync(hostId, cancellationToken);
            if (host is null)
            {
                _logger.LogWarning("Host {HostId} not found.", hostId);
                return HostErrors.HostNotFoundError;
            }

            var result = host.UpdateInstagramHandler(request.InstagramHandler);
            if (result.IsError)
            {
                _logger.LogWarning("UpdateInstagramHandler failed for Host {HostId}: {ErrorCode}", hostId, result.FirstError.Code);
                return result.FirstError;
            }

            try
            {
                await _repository.UpdateAsync(host, cancellationToken);
                _logger.LogInformation("Host {HostId} instagram handler updated to {InstagramHandler} successfully.", hostId, request.InstagramHandler);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating instagram handler for Host {HostId}.", hostId);
                return HostErrors.UnexpectedError;
            }

            return new UpdateInstagramHandlerOutputModel(host.Id, host.ContactInfo.InstagramHandler);
        }
    }
}
