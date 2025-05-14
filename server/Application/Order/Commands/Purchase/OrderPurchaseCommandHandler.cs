namespace Application.Order.Commands.Purchase
{
    using Application.Services.Contracts.Payment;
    using Application.Services.Contracts.User;
    using Complete;
    using Domain.Event;
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using Domain.Order.Builder;
    using Domain.Order.Error;
    using Domain.Order.Repository;
    using Domain.Venue.Error;
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts;

    internal class OrderPurchaseCommandHandler : IRequestHandler<OrderPurchaseCommand, ErrorOr<Success>>
    {
        private readonly IOrderDomainRepository _orderRepository;
        private readonly IEventDomainRepository _eventRepository;
        private readonly IVenueDomainRepository _venueRepository;
        private readonly IPaymentService _paymentService;
        private readonly IOrderBuilder _orderBuilder;
        private readonly ITicketBuilder _ticketBuilder;
        private readonly IMediator _mediator;
        private readonly ILogger<OrderPurchaseCommandHandler> _logger;
        private readonly ICurrentUser _currentUser;

        public OrderPurchaseCommandHandler(
            IOrderDomainRepository orderRepository,
            IEventDomainRepository eventRepository,
            IVenueDomainRepository venueRepository,
            IPaymentService paymentService,
            IOrderBuilder orderBuilder,
            ITicketBuilder ticketBuilder,
            IMediator mediator,
            ILogger<OrderPurchaseCommandHandler> logger,
            ICurrentUser currentUser)
        {
            _orderRepository = orderRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _paymentService = paymentService;
            _orderBuilder = orderBuilder;
            _ticketBuilder = ticketBuilder;
            _mediator = mediator;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<Success>> Handle(OrderPurchaseCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.BuyerId is null)
            {
                _logger.LogWarning("HostId claim is missing for user {UserId}", _currentUser.UserId);
                return EventErrors.Unauthorized;
            }

            var buyerId = _currentUser.BuyerId.Value;
            _logger.LogInformation("Handling CreateOrderCommand for Buyer {BuyerId}", buyerId);

            _logger.LogInformation(
                "Handling OrderPurchaseCommand for Buyer {BuyerId}, Event {EventId}",
                buyerId, request.EventId);
            var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (@event is null)
            {
                _logger.LogWarning("Event {EventId} not found.", request.EventId);
                return EventErrors.EventNotFoundError;
            }

            if (@event.Status != EventStatus.Active)
            {
                _logger.LogWarning(
                    "Event {EventId} is not active (Status: {Status}).",
                    request.EventId, @event.Status);
                return EventErrors.EventHasEndedOrCancelledError;
            }

            var venue = await _venueRepository.GetByIdAsync(@event.VenueId, cancellationToken);
            if (venue is null)
            {
                _logger.LogWarning("Venue {VenueId} not found.", @event.VenueId);
                return VenueErrors.VenueNotFoundError;
            }

            if (@event.TicketCount >= venue.Capacity)
            {
                _logger.LogWarning(
                    "No tickets left for Event {EventId} (Capacity: {Capacity}).",
                    request.EventId, venue.Capacity);
                return EventErrors.NoTicketsLeftError;
            }

            //TODO: create implementation of the payments service in the infrastructure layer when time comes.
            var paymentResult = await _paymentService.ProcessPaymentAsync(request.PaymentAmount, cancellationToken);
            if (paymentResult.IsError)
            {
                _logger.LogWarning(
                    "Payment failed for Buyer {BuyerId}: {ErrorCode}",
                    buyerId, paymentResult.FirstError.Code);
                return paymentResult.FirstError;
            }

            var orderResult = _orderBuilder
                .WithBuyer(buyerId)
                .Build();

            if (orderResult.IsError)
            {
                _logger.LogWarning(
                    "OrderBuilder failed for Buyer {BuyerId}: {ErrorCode}",
                    buyerId, orderResult.FirstError.Code);
                return orderResult.FirstError;
            }


            var order = orderResult.Value;

            var ticketResult = _ticketBuilder
                .WithEventId(request.EventId)
                .WithPrice(request.PaymentAmount)
                .WithType(request.TicketType)
                .Build();
            if (ticketResult.IsError)
            {
                _logger.LogWarning(
                    "TicketBuilder failed for Event {EventId}: {ErrorCode}",
                    request.EventId, ticketResult.FirstError.Code);
                return ticketResult.FirstError;
            }
            var ticket = ticketResult.Value;

            var addTicketResult = order.AddTicket(ticket);
            if (addTicketResult.IsError)
            {
                _logger.LogWarning(
                    "AddTicket failed for Order {OrderId}, Ticket {TicketId}: {ErrorCode}",
                    order.Id, ticket.Id, addTicketResult.FirstError.Code);
                return addTicketResult.FirstError;
            }

            try
            {
                await _orderRepository.AddAsync(order, cancellationToken);
                _logger.LogInformation(
                    "Order {OrderId} created and ticket added for Buyer {BuyerId}.",
                    order.Id, buyerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, "Error creating Order for Buyer {BuyerId}.", buyerId);
                return OrderError.UnexpectedError;
            }

            var completeResult = await _mediator.Send(new OrderCompleteCommand(order.Id), cancellationToken);
            if (completeResult.IsError)
            {
                _logger.LogWarning(
                    "OrderCompleteCommand failed for Order {OrderId}: {ErrorCode}",
                    order.Id, completeResult.FirstError.Code);
                return completeResult.FirstError;
            }
            _logger.LogInformation("Order {OrderId} completed successfully.", order.Id);

            return Result.Success;
        }
    }
}