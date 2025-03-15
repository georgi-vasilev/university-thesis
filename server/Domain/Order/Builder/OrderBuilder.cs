namespace Domain.Order.Builder
{
    using Error;
    using ErrorOr;

    internal class OrderBuilder : IOrderBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _buyerId = default!;

        public IOrderBuilder WithBuyer(Guid buyerId)
        {
            _buyerId = buyerId;
            return this;
        }

        public ErrorOr<Order> Build()
        {
            if(_buyerId == Guid.Empty)
            {
                return OrderError.InvalidBuyerError;
            }

            return new Order(_buyerId);
        }
    }
}
