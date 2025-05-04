namespace Domain.Order.Repository
{
    using Common;

    public interface IOrderDomainRepository : IDomainRepository<Order>
    {
        Task<Order> GetOrderAsync(Func<Order, bool> predicate);
        Task<Order?> GetByTransactionIdAsync(string transactionId, Guid buyerId, CancellationToken ct = default);
        Task<List<Order>> GetOrdersForEventAsync(Guid eventId);
    }
}
