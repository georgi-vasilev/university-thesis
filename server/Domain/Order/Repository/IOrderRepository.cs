namespace Domain.Order.Repository
{
    using Common;

    public interface IOrderRepository : IDomainRepository<Order>
    {
        Task<Order> GetOrderAsync(Func<Order, bool> predicate);
        Task<List<Order>> GetOrdersForEventAsync(Guid eventId);
    }
}
