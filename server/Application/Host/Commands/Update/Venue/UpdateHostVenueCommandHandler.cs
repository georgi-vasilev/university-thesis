namespace Application.Host.Commands.Update.Venue
{
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using Domain.Venue.Error;
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;

    public class UpdateHostVenueCommandHandler : IRequestHandler<UpdateHostVenueCommand, ErrorOr<UpdateHostVenueOutputModel>>
    {
        private readonly IHostDomainRepository _hostRepository;
        private readonly IVenueDomainRepository _venueRepository;

        public UpdateHostVenueCommandHandler(IHostDomainRepository repository, IVenueDomainRepository venueRepository)
        {
            _hostRepository = repository;
            _venueRepository = venueRepository;
        }

        public async Task<ErrorOr<UpdateHostVenueOutputModel>> Handle(UpdateHostVenueCommand request, CancellationToken cancellationToken)
        {
            var host = await _hostRepository.GetByIdAsync(request.Id, cancellationToken);
            if (host is null)
            {
                return HostErrors.HostNotFoundError;
            }

            var venue = await _venueRepository.GetByIdAsync(request.VenueId, cancellationToken);
            if (venue is null)
            {
                return VenueErrors.VenueNotFoundError;
            }

            var result = host.UpdateVenue(venue.Id);
            if (result.IsError)
            {
                return result.FirstError;
            }

            await _hostRepository.UpdateAsync(host, cancellationToken);

            return new UpdateHostVenueOutputModel(host.Id, host.VenueId);
        }
    }
}
