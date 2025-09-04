namespace Domain.Order.Builder
{
    using Common;

    public interface IOrderBuilder : IBuilder<Order>
    {
        IOrderBuilder WithBuyer(Guid buyerId);
        IOrderBuilder WithEvent(Guid eventId);
        IOrderBuilder WithTransaction(string transcationId);
    }
}
