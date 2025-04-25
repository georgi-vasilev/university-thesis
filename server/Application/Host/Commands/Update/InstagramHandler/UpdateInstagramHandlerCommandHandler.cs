namespace Application.Host.Commands.Update.InstagramHandler
{
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class UpdateInstagramHandlerCommandHandler : IRequestHandler<UpdateInstagramHandlerCommand, ErrorOr<UpdateInstagramHandlerOutputModel>>
    {
        private readonly IHostDomainRepository _repository;
        private readonly ILogger<UpdateInstagramHandlerCommandHandler> _logger;

        public UpdateInstagramHandlerCommandHandler(
            IHostDomainRepository repository,
            ILogger<UpdateInstagramHandlerCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<UpdateInstagramHandlerOutputModel>> Handle(
            UpdateInstagramHandlerCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateInstagramCommand for Host {HostId}", request.Id);
            var host = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (host is null)
            {
                _logger.LogWarning("Host {HostId} not found.", request.Id);
                return HostErrors.HostNotFoundError;
            }

            var result = host.UpdateInstagramHandler(request.InstagramHandler);
            if (result.IsError)
            {
                _logger.LogWarning("UpdateInstagramHandler failed for Host {HostId}: {ErrorCode}", request.Id, result.FirstError.Code);
                return result.FirstError;
            }

            try
            {
                await _repository.UpdateAsync(host, cancellationToken);
                _logger.LogInformation("Host {HostId} instagram handler updated to {InstagramHandler} successfully.", request.Id, request.InstagramHandler);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating instagram handler for Host {HostId}.", request.Id);
                return HostErrors.UnexpectedError;
            }

            return new UpdateInstagramHandlerOutputModel(host.Id, host.ContactInfo.InstagramHandler);
        }
    }
}
