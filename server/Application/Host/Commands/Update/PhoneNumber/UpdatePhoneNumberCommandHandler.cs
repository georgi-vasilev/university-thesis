namespace Application.Host.Commands.Update.PhoneNumber
{
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class UpdatePhoneNumberCommandHandler : IRequestHandler<UpdatePhoneNumberCommand, ErrorOr<UpdatePhoneNumberOutputModel>>
    {
        private readonly IHostDomainRepository _repository;
        private readonly ILogger<UpdatePhoneNumberCommandHandler> _logger;

        public UpdatePhoneNumberCommandHandler(
            IHostDomainRepository repository,
            ILogger<UpdatePhoneNumberCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<UpdatePhoneNumberOutputModel>> Handle(
            UpdatePhoneNumberCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdatePhoneNumberCommand for Host {HostId}", request.Id);
            var host = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (host is null)
            {
                _logger.LogWarning("Host {HostId} not found.", request.Id);
                return HostErrors.HostNotFoundError;
            }

            var result = host.UpdatePhoneNumber(request.PhoneNumber);
            if (result.IsError)
            {
                _logger.LogWarning("UpdatePhoneNumber failed for Host {HostId}: {ErrorCode}", request.Id, result.FirstError.Code);
                return result.FirstError;
            }

            try
            {
                await _repository.UpdateAsync(host, cancellationToken);
                _logger.LogInformation("Host {HostId} phone number updated to {PhoneNumber} successfully.", request.Id, request.PhoneNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating phone number for Host {HostId}.", request.Id);
                return HostErrors.UnexpectedError;
            }

            return new UpdatePhoneNumberOutputModel(host.Id, host.ContactInfo.PhoneNumber);
        }
    }
}