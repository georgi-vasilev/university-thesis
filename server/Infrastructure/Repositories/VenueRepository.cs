namespace Infrastructure.Repositories
{
    using Domain.Venue;
    using Domain.Venue.Repository;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    internal class VenueRepository : IVenueDomainRepository
    {
        private readonly IApplicationDbContext _context;

        public VenueRepository(IApplicationDbContext context)
            => _context = context;

        public async Task AddAsync(Venue aggregate, CancellationToken cancellationToken)
        {
            _context.Venue.Add(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Venue.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is not null)
            {
                _context.Venue.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Venue
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Venue aggregate, CancellationToken cancellationToken)
        {
            _context.Venue.Update(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
