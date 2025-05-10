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

        public async Task<ErrorOr<Success>> CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order is null)
            {
                return OrderError.OrderNotFoundError;
            }

            var completeResult = order.CompleteOrder();
            if (completeResult.IsError)
            {
                return completeResult.FirstError;
            }

            await _orderRepository.UpdateAsync(order, cancellationToken);

            // TODO: dispatch event
            return Result.Success;
        }


        public async Task<ErrorOr<Success>> PurchaseTicketAsync(
            Guid buyerId,
            Guid eventId,
            Money price,
            TicketType type, 
            CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
            if (@event is null)
            {
                return EventErrors.EventNotFoundError;
            }

            var venue = await _venueRepository.GetByIdAsync(@event.VenueId, cancellationToken);
            if(venue is null)
            {
                return VenueErrors.VenueNotFoundError;
            }

            if (@event.TicketCount >= venue.Capacity)
            {
                return EventErrors.NoTicketsLeftError;
            }

            if(@event.Status != EventStatus.Active)
            {
                return EventErrors.EventHasEndedOrCancelledError;
            }


            var orderBuild = _orderBuilder.WithBuyer(buyerId).Build();
            if (orderBuild.IsError)
            {
                return orderBuild.FirstError;
            }
            var order = orderBuild.Value;

            var ticketBuilderResult = _ticketBuilder
                .WithEventId(eventId)
                .WithPrice(price)
                .WithType(type)
                .Build();

            if (ticketBuilderResult.IsError)
            {
                return ticketBuilderResult.FirstError;
            }

            var ticket = ticketBuilderResult.Value;

            var result = order.AddTicket(ticket);
            if (result.IsError)
            {
                return result.FirstError;
            }

            await _orderRepository.UpdateAsync(order, cancellationToken);

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
