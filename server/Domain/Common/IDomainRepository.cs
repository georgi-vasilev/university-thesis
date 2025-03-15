namespace Domain.Common
{
    public interface IDomainRepository<T> where T : IAggregateRoot
    {
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T aggregate);
        Task UpdateAsync(T aggregate);
        Task DeleteAsync(Guid id);
    }
}
