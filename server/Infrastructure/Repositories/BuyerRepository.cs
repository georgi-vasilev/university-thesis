namespace Infrastructure.Repositories
{
    using Domain.Buyer;
    using Domain.Buyer.Repository;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    internal class BuyerRepository : IBuyerDomainRepository
    {
        private readonly IApplicationDbContext _context;

        public BuyerRepository(IApplicationDbContext context) 
            => _context = context;

        public async Task AddAsync(Buyer buyer, CancellationToken cancellationToken)
        {
            await _context.Buyers.AddAsync(buyer);
            await _context.SaveChangesAsync();
        }

        public async Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken) 
            => await _context.Buyers.FirstOrDefaultAsync(b => b.Email == email, cancellationToken);
    }
}
