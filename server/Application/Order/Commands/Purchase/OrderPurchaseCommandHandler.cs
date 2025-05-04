namespace Application.Order.Commands.Purchase
{
    using Domain.Event.Error;
    using Domain.Event.Service;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts.User;

    internal class OrderPurchaseCommandHandler : IRequestHandler<OrderPurchaseCommand, ErrorOr<Success>>
    {
        private readonly ILogger<OrderPurchaseCommandHandler> _logger;
        private readonly IEventOrderService _eventOrderService;
        private readonly ICurrentUser _currentUser;

        public OrderPurchaseCommandHandler(
            ILogger<OrderPurchaseCommandHandler> logger,
            ICurrentUser currentUser,
            IEventOrderService eventOrderService)
        {
            _logger = logger;
            _currentUser = currentUser;
            _eventOrderService = eventOrderService;
        }


        public async Task<ErrorOr<Success>> Handle(OrderPurchaseCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.BuyerId is null)
            {
                _logger.LogWarning("BuyerId claim is missing for user {UserId}", _currentUser.UserId);
                return EventErrors.Unauthorized;
            }

            var buyerId = _currentUser.BuyerId.Value;
            _logger.LogInformation("Handling OrderPurchaseCommand for Buyer {BuyerId}, Event {EventId}",
                buyerId, request.EventId);

            return await _eventOrderService.PurchaseTicketAsync(
                buyerId,
                request.EventId,
                request.PaymentIntentId,
                request.Quantity,
                request.TicketType,
                cancellationToken);
        }
    }
}