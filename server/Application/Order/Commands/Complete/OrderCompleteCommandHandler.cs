namespace Application.Order.Commands.Complete
{
    using Domain.Order.Error;
    using Domain.Order.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class OrderCompleteCommandHandler : IRequestHandler<OrderCompleteCommand, ErrorOr<OrderCompleteOutputModel>>
    {
        private readonly IOrderDomainRepository _orderRepository;
        private readonly ILogger<OrderCompleteCommandHandler> _logger;

        public OrderCompleteCommandHandler(
            IOrderDomainRepository orderRepository,
            ILogger<OrderCompleteCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<OrderCompleteOutputModel>> Handle(OrderCompleteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling OrderCompleteCommand for Order {OrderId}", request.OrderId);
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
            {
                _logger.LogWarning("Order {OrderId} not found.", request.OrderId);
                return OrderError.OrderNotFoundError;
            }

            var result = order.CompleteOrder();
            if (result.IsError)
            {
                _logger.LogWarning(
                    "CompleteOrder failed for Order {OrderId}: {ErrorCode}",
                    request.OrderId,
                    result.FirstError.Code);
                return result.FirstError;
            }

            try
            {
                await _orderRepository.UpdateAsync(order, cancellationToken);
                _logger.LogInformation("Order {OrderId} marked as Completed.", request.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Order {OrderId} to Completed.", request.OrderId);
                return OrderError.UnexpectedError;
            }

            return new OrderCompleteOutputModel(order.Id, order.Status);
        }
    }
}
