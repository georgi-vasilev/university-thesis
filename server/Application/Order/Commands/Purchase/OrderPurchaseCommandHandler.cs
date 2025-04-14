namespace Application.Order.Commands.Purchase
{
    using Complete;
    using Domain.Event;
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using Domain.Order.Builder;
    using Domain.Order.Repository;
    using Domain.Venue.Error;
    using Domain.Venue.Repository;
    using ErrorOr;
    using MediatR;
    using Services.Contracts;

    internal class OrderPurchaseCommandHandler : IRequestHandler<OrderPurchaseCommand, ErrorOr<Success>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventDomainRepository _eventRepository;
        private readonly IVenueDomainRepository _venueRepository;
        private readonly IPaymentService _paymentService;

        private readonly IOrderBuilder _orderBuilder;
        private readonly ITicketBuilder _ticketBuilder;
        private readonly IMediator _mediator;

        public OrderPurchaseCommandHandler(
            IOrderRepository orderRepository,
            IEventDomainRepository eventRepository,
            IVenueDomainRepository venueRepository,
            IPaymentService paymentService,
            IOrderBuilder orderBuilder,
            ITicketBuilder ticketBuilder,
            IMediator mediator)
        {
            _orderRepository = orderRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _paymentService = paymentService;
            _orderBuilder = orderBuilder;
            _ticketBuilder = ticketBuilder;
            _mediator = mediator;
        }

        public async Task<ErrorOr<Success>> Handle(OrderPurchaseCommand request, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (@event is null)
            {
                return EventErrors.EventNotFoundError;
            }

            if (@event.Status != EventStatus.Active)
            {
                return EventErrors.EventHasEndedOrCancelledError;
            }

            var venue = await _venueRepository.GetByIdAsync(@event.VenueId, cancellationToken);
            if (venue is null)
            {
                return VenueErrors.VenueNotFoundError;
            }

            if (@event.TicketCount >= venue.Capacity)
            {
                return EventErrors.NoTicketsLeftError;
            }

            //TODO: create implementation of the payments service in the infrastructure layer when time comes.
            var paymentResult = await _paymentService.ProcessPaymentAsync(request.PaymentAmount, cancellationToken);
            if (paymentResult.IsError)
            {
                return paymentResult.FirstError;
            }

            var orderResult = _orderBuilder
                .WithBuyer(request.BuyerId)
                .Build();

            if (orderResult.IsError)
            {
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
                return ticketResult.FirstError;
            }
            var ticket = ticketResult.Value;

            var addTicketResult = order.AddTicket(ticket);
            if (addTicketResult.IsError)
            {
                return addTicketResult.FirstError;
            }

            await _orderRepository.AddAsync(order, cancellationToken);

            var completeResult = await _mediator.Send(new OrderCompleteCommand(order.Id), cancellationToken);
            if (completeResult.IsError)
            {
                return completeResult.FirstError;
            }

            return Result.Success;
        }
    }
}