namespace Application.Host.Commands.Update.Venue
{
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using Domain.Venue.Error;
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class UpdateHostVenueCommandHandler : IRequestHandler<UpdateHostVenueCommand, ErrorOr<UpdateHostVenueOutputModel>>
    {
        private readonly IHostDomainRepository _hostRepository;
        private readonly IVenueDomainRepository _venueRepository;
        private readonly ILogger<UpdateHostVenueCommandHandler> _logger;

        public UpdateHostVenueCommandHandler(
            IHostDomainRepository repository,
            IVenueDomainRepository venueRepository,
            ILogger<UpdateHostVenueCommandHandler> logger)
        {
            _hostRepository = repository;
            _venueRepository = venueRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<UpdateHostVenueOutputModel>> Handle(UpdateHostVenueCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateHostVenueCommand for Host {HostId}", request.Id);
            var host = await _hostRepository.GetByIdAsync(request.Id, cancellationToken);
            if (host is null)
            {
                _logger.LogWarning("Host {HostId} not found.", request.Id);
                return HostErrors.HostNotFoundError;
            }

            var venue = await _venueRepository.GetByIdAsync(request.VenueId, cancellationToken);
            if (venue is null)
            {
                _logger.LogWarning("Venue {VenueId} not found.", request.VenueId);
                return VenueErrors.VenueNotFoundError;
            }

            var result = host.UpdateVenue(venue.Id);
            if (result.IsError)
            {
                _logger.LogWarning("UpdateVenue failed for Host {HostId}: {ErrorCode}", request.Id, result.FirstError.Code);
                return result.FirstError;
            }

            try
            {
                await _hostRepository.UpdateAsync(host, cancellationToken);
                _logger.LogInformation("Host {HostId} venue updated venue to {VenueId} successfully.", request.Id, request.VenueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating venue for Host {HostId}.", request.Id);
                return HostErrors.UnexpectedError;
            }

            return new UpdateHostVenueOutputModel(host.Id, host.VenueId!.Value);
        }
    }
}
