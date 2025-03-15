namespace Domain.Order.Builder
{
    using Common.ValueObject;
    using Domain.Common.Error;
    using Domain.Order.Error;
    using ErrorOr;

    internal class TicketBuilder
    {
        private Guid _eventId;
        private Money _price = default!;
        private TicketType _ticketType;

        public TicketBuilder WithEventId(Guid eventId)
        {
            _eventId = eventId;
            return this;
        }

        public TicketBuilder WithPrice(Money price)
        {
            _price = price;
            return this;
        }

        public TicketBuilder WithType(TicketType type)
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
