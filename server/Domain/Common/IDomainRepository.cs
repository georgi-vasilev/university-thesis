namespace Domain.Common
{
    public interface IDomainRepository<T> where T : IAggregateRoot
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(T aggregate, CancellationToken cancellationToken);
        Task UpdateAsync(T aggregate, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
