namespace Domain.Event.Service
{
    using Common.ValueObject;
    using Error;
    using ErrorOr;
    using Host;
    using Order;
    using Order.Builder;
    using Order.Error;
    using Order.Repository;
    using Repository;
    using System.Threading;
    using Venue.Error;
    using Venue.Repository;

    internal class EventOrderService : IEventOrderService
    {
        private readonly IEventDomainRepository _eventRepository;
        private readonly IOrderDomainRepository _orderRepository;
        private readonly IVenueDomainRepository _venueRepository;
        private readonly IOrderBuilder _orderBuilder;
        private readonly ITicketBuilder _ticketBuilder;

        public EventOrderService(
            IEventDomainRepository eventRepository,
            IOrderDomainRepository orderRepository,
            IOrderBuilder orderBuilder,
            ITicketBuilder ticketBuilder,
            IVenueDomainRepository venueRepository)
        {
            _eventRepository = eventRepository;
            _orderRepository = orderRepository;
            _orderBuilder = orderBuilder;
            _ticketBuilder = ticketBuilder;
            _venueRepository = venueRepository;
        }

        public async Task<ErrorOr<Success>> CancelTicketOrderAsync(Guid buyerId, Guid ticketId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository
                .GetOrderAsync(order => 
                    order.BuyerId == buyerId &&
                    order.Tickets.Any(t => t.Id == ticketId));

            if (order is null)
            {
                return OrderError.OrderNotFoundError;
            }

            if (order.Status == OrderStatus.Completed)
            {
                return OrderError.OrderAlreadyCompletedError;
            }

            var ticket = order.Tickets.FirstOrDefault(t => t.Id == ticketId);
            if (ticket is null)
            {
                return OrderError.TicketNotFoundError;
            }

            if (ticket.HasBeenUsed)
            {
                return OrderError.TicketAlreadyUsedError;
            }

            var removeResult = order.RemoveTicket(ticketId);
            if (removeResult.IsError)
            {
                return removeResult.FirstError;
            }

            if (!order.Tickets.Any())
            {
                var statusResult = order.ChangeOrderStatus(OrderStatus.Cancelled);
                if (statusResult.IsError)
                {
                    return statusResult.FirstError;
                }
            }

            await _orderRepository.UpdateAsync(order, cancellationToken);

            // TODO; dispatch event.
            return Result.Success;
        }


        public async Task<ErrorOr<Success>> ChangeEventVenueAsync(Host host, Guid eventId, Guid newVenueId, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
            if (@event is null)
            {
                return EventErrors.EventNotFoundError;
            }

            if (host.Id != @event.HostId)
            {
                return EventErrors.EventDoesNotBelongToHostError;
            }

            var venue = await _venueRepository.GetByIdAsync(newVenueId, cancellationToken);

            if(venue is null)
            {
                return VenueErrors.VenueNotFoundError;
            }
            
            if(@event.TicketCount > venue.Capacity)
            {
                return EventErrors.CapacityExceededError;
            }

            var result = @event.UpdateDetails(
                @event.Name,
                @event.Description,
                @event.Date,
                @event.Time,
                newVenueId);

            if (result.IsError)
            {
                return result.FirstError;
            }

            host.UpdateVenue(newVenueId);

            // TODO: dispatch event

            return Result.Success;
        }

        public async Task<ErrorOr<Order>> CompleteOrderAsync(
            Guid eventId,
            Guid buyerId,
            string transactionId,
            string paymentIntentStatus,
            long amount,
            CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByTransactionIdAsync(transactionId, buyerId, cancellationToken);
            if (order is null)
            {
                return OrderError.OrderNotFoundError;
            }

            if (order.Payment!.Status == PaymentStatus.Completed || order.Status == OrderStatus.Completed)
            {
                return OrderError.OrderAlreadyCompletedError;
            }

            if (!string.Equals(paymentIntentStatus, "succeeded", StringComparison.OrdinalIgnoreCase))
            {
                return OrderError.CannotChangeOrderStatusError;
            }

            var expectedCents = (long)(order.Payment.Amount * 100m);
            var actualCents = amount;

            if (expectedCents != actualCents)
            {

                order.ChangeOrderStatus(OrderStatus.Cancelled);
                await _orderRepository.UpdateAsync(order, cancellationToken);


                return OrderError.UnexpectedError;
            }

            var @event = await _eventRepository.GetByIdAsync(eventId, cancellationToken);

            if (@event is null)
            {
                return EventErrors.EventNotFoundError;
            }

            var venue = await _venueRepository.GetByIdAsync(@event.VenueId, cancellationToken);
            var result = order.CompleteOrder();
            if (result.IsError)
            {
                return result.FirstError;
            }

            try
            {
                await _orderRepository.UpdateAsync(order, cancellationToken);

                return order;
            }
            catch (Exception ex)
            {
                return OrderError.UnexpectedError;
            }
        }


        public async Task<ErrorOr<Success>> PurchaseTicketAsync(
            Guid buyerId,
            Guid eventId,
            string transactionId,
            int quantity,
            TicketType type, 
            CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
            if (@event is null)
            {
                return EventErrors.EventNotFoundError;
            }
            else if (@event.Status != EventStatus.Active)
            {
                return EventErrors.EventHasEndedOrCancelledError;
            }

            var venue = await _venueRepository.GetByIdAsync(@event.VenueId, cancellationToken);
            if(venue is null)
            {
                return VenueErrors.VenueNotFoundError;
            } 
            else if (@event.TicketCount >= venue.Capacity)
            {
                return EventErrors.NoTicketsLeftError;
            }

            var unitPrice = type == TicketType.VIP ? @event.VipPrice! : @event.GeneralPrice;
            var total = new Money(unitPrice.Amount * quantity, unitPrice.Currency);

            var orderBuild = _orderBuilder
                .WithBuyer(buyerId)
                .WithEvent(eventId)
                .WithTransaction(transactionId)
                .Build();
            if (orderBuild.IsError)
            {
                return orderBuild.FirstError;
            }
            var order = orderBuild.Value;

            for (int i = 0; i < quantity; i++)
            {
                var perTicket = new Money(unitPrice.Amount, unitPrice.Currency);

                var ticketResult = _ticketBuilder
                    .WithEventId(eventId)
                    .WithPrice(perTicket)
                    .WithType(type)
                    .Build();

                if (ticketResult.IsError)
                {
                    return ticketResult.FirstError;
                }

                var ticketToAdd = ticketResult.Value;

                var addToBuyer = ticketToAdd.AddToBuyer(buyerId);

                if (addToBuyer.IsError)
                {
                    return addToBuyer.FirstError;
                }

                var addTicketResult = order.AddTicket(ticketToAdd);
                if (addTicketResult.IsError)
                {
                    return addTicketResult.FirstError;
                }

                var addTicketToEventResult = @event.AddTicket(ticketToAdd.Id, venue.Capacity);
                if (addTicketToEventResult.IsError)
                {
                    return addTicketToEventResult.FirstError;
                }
            }


            var paymentDetails = new PaymentDetails(
                total.Amount,
                "stripe",
                PaymentStatus.Pending,
                transactionId);

            var updatePaymentResult = order.UpdatePaymentDetails(paymentDetails);
            if (updatePaymentResult.IsError)
            {
                return updatePaymentResult.FirstError;
            }

            await _orderRepository.AddAsync(order, cancellationToken);
            await _eventRepository.UpdateAsync(@event, cancellationToken);

            // TODO: domain event
            return Result.Success;

        }

        public async Task<ErrorOr<Success>> UpdateEventDetailsAsync(Host host, Event updatedEvent, CancellationToken cancellationToken)
        {
            if (host.Id != updatedEvent.HostId)
            {
                return EventErrors.EventDoesNotBelongToHostError;
            }

            var existingEvent = await _eventRepository.GetByIdAsync(updatedEvent.Id, cancellationToken);
            if (existingEvent is null)
            {
                return EventErrors.EventNotFoundError;
            }

            var updateResult = existingEvent.UpdateDetails(
                                    updatedEvent.Name,
                                    updatedEvent.Description,
                                    updatedEvent.Date,
                                    updatedEvent.Time,
                                    updatedEvent.VenueId);
            if (updateResult.IsError)
            {
                return updateResult.FirstError;
            }

            await _eventRepository.UpdateAsync(existingEvent, cancellationToken);

            //TODO: dispatch event
            return Result.Success;
        }
        
    }
}
