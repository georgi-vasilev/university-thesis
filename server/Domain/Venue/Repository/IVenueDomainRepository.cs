namespace Domain.Venue.Repository
{
    using Common;

    public interface IVenueDomainRepository : IDomainRepository<Venue>
    {
        Task<IEnumerable<Venue>> GetAllAsync(CancellationToken cancellationToken);
    }
}
