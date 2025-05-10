namespace Domain.Buyer.Repository
{
    public interface IBuyerDomainRepository
    {
        Task AddAsync(Buyer buyer, CancellationToken cancellationToken);
        Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
