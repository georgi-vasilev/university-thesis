namespace Application.Host.Commands.Update.Email
{
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, ErrorOr<UpdateEmailOutputModel>>
    {
        private readonly IHostDomainRepository _repository;
        private readonly ILogger<UpdateEmailCommandHandler> _logger;

        public UpdateEmailCommandHandler(
            IHostDomainRepository repository,
            ILogger<UpdateEmailCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<UpdateEmailOutputModel>> Handle(
            UpdateEmailCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateEmailCommand for Host {HostId}", request.Id);

            var host = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (host is null) 
            {
                _logger.LogWarning("Host {HostId} not found.", request.Id);
                return HostErrors.HostNotFoundError;
            }

            var updateResult = host.UpdateEmail(request.Email);
            if (updateResult.IsError)
            {
                _logger.LogWarning("UpdateEmail failed for Host {HostId}: {ErrorCode}", request.Id, updateResult.FirstError.Code);
                return updateResult.FirstError;
            }

            try
            {
                await _repository.UpdateAsync(host, cancellationToken);
                _logger.LogInformation("Host {HostId} email updated to {Email} successfully.", request.Id, request.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating email for Host {HostId}.", request.Id);
                return HostErrors.UnexpectedError;
            }

            return new UpdateEmailOutputModel(host.Id, host.ContactInfo.Email);
        }
    }
}
