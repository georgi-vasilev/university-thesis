namespace Infrastructure.Repositories
{
    using Domain.Order;
    using Domain.Order.Repository;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    internal class OrderRepository : IOrderDomainRepository
    {
        private readonly IApplicationDbContext _context;

        public OrderRepository(IApplicationDbContext context) 
            => _context = context;

        public async Task AddAsync(Order aggregate, CancellationToken cancellationToken)
        {
            _context.Orders.Add(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
            if (entity is not null)
            {
                _context.Orders.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Include(x => x.Tickets)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<Order?> GetOrderAsync(Func<Order, bool> predicate)
        {
            var all = await _context.Orders
                .Include(x => x.Tickets)
                .AsNoTracking()
                .ToListAsync();

            return all.FirstOrDefault(predicate);
        }

        public async Task<List<Order>> GetOrdersForEventAsync(Guid eventId)
        {
            var all = await _context.Orders
                .Include(x => x.Tickets)
                .AsNoTracking()
                .ToListAsync();

            return all
                .Where(o => o.Tickets.Any(t => t.EventId == eventId))
                .ToList();
        }

        public async Task UpdateAsync(Order aggregate, CancellationToken cancellationToken)
        {
            _context.Orders.Update(aggregate);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}