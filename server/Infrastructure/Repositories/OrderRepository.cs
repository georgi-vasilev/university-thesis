namespace Infrastructure.Repositories
{
    using Domain.Order;
    using Domain.Order.Repository;

    public class OrderRepository : IOrderDomainRepository
    {
        public Task AddAsync(Order aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderAsync(Func<Order, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrdersForEventAsync(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Order aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
