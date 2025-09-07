namespace Application.Order.Commands.Fail
{
    using Domain.Order;
    using Domain.Order.Error;
    using Domain.Order.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;

    internal class MarkOrderFailedCommandHandler : IRequestHandler<MarkOrderFailedCommand, ErrorOr<Success>>
    {
        private readonly IOrderDomainRepository _orderRepository;
        private readonly ILogger<MarkOrderFailedCommand> _logger;

        public MarkOrderFailedCommandHandler(
            IOrderDomainRepository orderRepository,
            ILogger<MarkOrderFailedCommand> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<Success>> Handle(
            MarkOrderFailedCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Handling MarkPaymentFailedCommand for TransactionId {TransactionId}",
                request.TransactionId);

            var order = await _orderRepository.GetOrderAsync(o =>
                o.Payment != null && o.Payment.TransactionId == request.TransactionId);

            if (order == null)
            {
                _logger.LogWarning(
                    "Order not found for TransactionId {TransactionId}",
                    request.TransactionId);
                return OrderError.OrderNotFoundError;
            }

            var failedPaymentDetails = new PaymentDetails(
                order.Payment?.Amount ?? 0,
                "stripe",
                PaymentStatus.Failed,
                request.TransactionId);

            var result = order.MarkPaymentAsFailed(failedPaymentDetails);
            if (result.IsError)
            {
                _logger.LogWarning(
                    "MarkPaymentAsFailed failed for Order {OrderId}: {ErrorCode}",
                    order.Id, result.FirstError.Code);
                return result.FirstError;
            }

            try
            {
                await _orderRepository.UpdateAsync(order, cancellationToken);
                _logger.LogInformation(
                    "Order {OrderId} payment marked as failed for TransactionId {TransactionId}. Reason: {FailureReason}",
                    order.Id, request.TransactionId, request.FailureReason);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating Order {OrderId} payment status", order.Id);
                return OrderError.UnexpectedError;
            }

            return Result.Success;
        }
    }
}

