namespace Domain.Order.Builder
{
    using Common.Error;
    using Common.ValueObject;
    using Error;
    using ErrorOr;

    internal class TicketBuilder : ITicketBuilder
    {
        private Guid _eventId;
        private Money _price = default!;
        private TicketType _ticketType;

        public ITicketBuilder WithEventId(Guid eventId)
        {
            _eventId = eventId;
            return this;
        }

        public ITicketBuilder WithPrice(Money price)
        {
            _price = price;
            return this;
        }

        public ITicketBuilder WithType(TicketType type)
        {
            _ticketType = type;
            return this;
        }

        public ErrorOr<Ticket> Build()
        {
            if(_eventId == Guid.Empty)
            {
                return TicketError.InvalidEventError;
            }

            if (_price is null)
            {
                return MoneyError.NullError;
            }

            if (_price.Amount  <= 0)
            {
                return MoneyError.InvalidAmountError;
            }

            return new Ticket(_eventId, _price, _ticketType);
        }
    }
}
