namespace Application.Host.Commands.Update.Venue
{
    using Domain.Event.Error;
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using Domain.Venue.Error;
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts.User;

    public class UpdateHostVenueCommandHandler : IRequestHandler<UpdateHostVenueCommand, ErrorOr<UpdateHostVenueOutputModel>>
    {
        private readonly IHostDomainRepository _hostRepository;
        private readonly IVenueDomainRepository _venueRepository;
        private readonly ILogger<UpdateHostVenueCommandHandler> _logger;
        private readonly ICurrentUser _currentUser;

        public UpdateHostVenueCommandHandler(
            IHostDomainRepository repository,
            IVenueDomainRepository venueRepository,
            ILogger<UpdateHostVenueCommandHandler> logger,
            ICurrentUser currentUser)
        {
            _hostRepository = repository;
            _venueRepository = venueRepository;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<UpdateHostVenueOutputModel>> Handle(UpdateHostVenueCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.HostId is null)
            {
                _logger.LogWarning("HostId claim is missing for user {UserId}", _currentUser.UserId);
                return EventErrors.Unauthorized;
            }

            var hostId = _currentUser.HostId.Value;

            _logger.LogInformation("Handling UpdateHostVenueCommand for Host {HostId}", hostId);
            var host = await _hostRepository.GetByIdAsync(hostId, cancellationToken);
            if (host is null)
            {
                _logger.LogWarning("Host {HostId} not found.", hostId);
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
                _logger.LogWarning("UpdateVenue failed for Host {HostId}: {ErrorCode}", hostId, result.FirstError.Code);
                return result.FirstError;
            }

            try
            {
                await _hostRepository.UpdateAsync(host, cancellationToken);
                _logger.LogInformation("Host {HostId} venue updated venue to {VenueId} successfully.", hostId, request.VenueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating venue for Host {HostId}.", hostId);
                return HostErrors.UnexpectedError;
            }

            return new UpdateHostVenueOutputModel(host.Id, host.VenueId!.Value);
        }
    }
}
