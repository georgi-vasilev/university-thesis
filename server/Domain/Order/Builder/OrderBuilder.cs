namespace Domain.Order.Builder
{
    using Error;
    using ErrorOr;

    internal class OrderBuilder : IOrderBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _buyerId = default!;
        private Guid _eventId = default!;
        private string _transcationId = default!;

        public IOrderBuilder WithBuyer(Guid buyerId)
        {
            _buyerId = buyerId;
            return this;
        }
        public IOrderBuilder WithEvent(Guid eventId)
        {
            _eventId = eventId;
            return this;
        }

        public IOrderBuilder WithTransaction(string transcationId)
        {
            _transcationId = transcationId;
            return this;
        }

        public ErrorOr<Order> Build()
        {
            if(_buyerId == Guid.Empty)
            {
                return OrderError.InvalidBuyerError;
            }

            return new Order(_buyerId, _eventId);
        }
    }
}
