namespace Application.Host.Commands.Update.PhoneNumber
{
    using Domain.Event.Error;
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts.User;

    public class UpdatePhoneNumberCommandHandler : IRequestHandler<UpdatePhoneNumberCommand, ErrorOr<UpdatePhoneNumberOutputModel>>
    {
        private readonly IHostDomainRepository _repository;
        private readonly ILogger<UpdatePhoneNumberCommandHandler> _logger;
        private readonly ICurrentUser _currentUser;

        public UpdatePhoneNumberCommandHandler(
            IHostDomainRepository repository,
            ILogger<UpdatePhoneNumberCommandHandler> logger,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<UpdatePhoneNumberOutputModel>> Handle(
            UpdatePhoneNumberCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUser.HostId is null)
            {
                _logger.LogWarning("HostId claim is missing for user {UserId}", _currentUser.UserId);
                return EventErrors.Unauthorized;
            }

            var hostId = _currentUser.HostId.Value;
            _logger.LogInformation("Handling UpdatePhoneNumberCommand for Host {HostId}", hostId);
            var host = await _repository.GetByIdAsync(hostId, cancellationToken);
            if (host is null)
            {
                _logger.LogWarning("Host {HostId} not found.", hostId);
                return HostErrors.HostNotFoundError;
            }

            var result = host.UpdatePhoneNumber(request.PhoneNumber);
            if (result.IsError)
            {
                _logger.LogWarning("UpdatePhoneNumber failed for Host {HostId}: {ErrorCode}", hostId, result.FirstError.Code);
                return result.FirstError;
            }

            try
            {
                await _repository.UpdateAsync(host, cancellationToken);
                _logger.LogInformation("Host {HostId} phone number updated to {PhoneNumber} successfully.", hostId, request.PhoneNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating phone number for Host {HostId}.", hostId);
                return HostErrors.UnexpectedError;
            }

            return new UpdatePhoneNumberOutputModel(host.Id, host.ContactInfo.PhoneNumber);
        }
    }
}