namespace Infrastructure.Repositories
{
    using Domain.Host;
    using Domain.Host.Repository;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class HostRepository : IHostDomainRepository
    {
        public Task AddAsync(Host aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Host?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Host aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
