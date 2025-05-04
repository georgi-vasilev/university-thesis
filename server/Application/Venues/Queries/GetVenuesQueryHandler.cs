namespace Application.Venues.Queries
{
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;

    internal class GetVenuesQueryHandler : IRequestHandler<GetVenuesQuery, ErrorOr<IEnumerable<GetVenuesOutputModel>>>
    {
        private readonly IVenueDomainRepository _venueRepository;
        public GetVenuesQueryHandler(IVenueDomainRepository venueRepository)
            => _venueRepository = venueRepository;

        public async Task<ErrorOr<IEnumerable<GetVenuesOutputModel>>> Handle(GetVenuesQuery request, CancellationToken cancellationToken)
        {
            var venues = await _venueRepository.GetAllAsync(cancellationToken);

            return venues
                .Select(x => new GetVenuesOutputModel(x.Id, x.Name))
                .ToList();
        }
    }
}
