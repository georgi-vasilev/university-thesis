namespace Domain.Host.Repository
{
    using Common;

    public interface IHostDomainRepository : IDomainRepository<Host>
    {
        Task<Host?> GetHostByEmailAsync(string email, CancellationToken cancellationToken);

    }
}
