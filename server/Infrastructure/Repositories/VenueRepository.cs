namespace Infrastructure.Repositories
{
    using Domain.Venue;
    using Domain.Venue.Repository;

    public class VenueRepository : IVenueDomainRepository
    {
        public Task AddAsync(Venue aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Venue aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
