namespace Application.Order.Commands.Complete
{
    using Domain.Event.Service;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OrderCompleteCommandHandler : IRequestHandler<OrderCompleteCommand, ErrorOr<OrderCompleteOutputModel>>
    {
        private readonly IEventOrderService _eventOrderService;
        private readonly ILogger<OrderCompleteCommandHandler> _logger;

        public OrderCompleteCommandHandler(
            IEventOrderService eventOrderService,
            ILogger<OrderCompleteCommandHandler> logger)
        {
            _eventOrderService = eventOrderService;
            _logger = logger;
        }

        public async Task<ErrorOr<OrderCompleteOutputModel>> Handle(OrderCompleteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling OrderCompleteCommand for Order {OrderId}", request.TranscationId);
           
            var result = await _eventOrderService.CompleteOrderAsync(
                Guid.Parse(request.EventId),
                Guid.Parse(request.BuyerId),
                request.TranscationId,
                request.PaymentIntentStatus,
                request.Amount,
                cancellationToken);

            if (result.IsError)
            {
                return result.FirstError;
            }

            var order = result.Value;

            return new OrderCompleteOutputModel(order.Id, order.Status);
        }
    }
}
