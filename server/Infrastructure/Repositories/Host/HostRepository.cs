namespace Infrastructure.Repositories.Host
{
    using Domain.Host;
    using Domain.Host.Repository;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class HostRepository : IHostDomainRepository
    {
        private readonly IApplicationDbContext _context;

        public HostRepository(IApplicationDbContext context)
            => _context = context;

        public async Task AddAsync(Host aggregate, CancellationToken cancellationToken)
        {
            _context.Hosts.Add(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Hosts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is not null)
            {
                _context.Hosts.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<Host?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Hosts
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Host aggregate, CancellationToken cancellationToken)
        {
            _context.Hosts.Update(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
