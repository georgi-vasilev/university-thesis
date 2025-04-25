namespace Application.Order.Commands.Create
{
    using Domain.Order.Builder;
    using Domain.Order.Error;
    using Domain.Order.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ErrorOr<CreateOrderOutputModel>>
    {
        private readonly IOrderDomainRepository _orderRepository;
        private readonly IOrderBuilder _orderBuilder;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(
            IOrderDomainRepository orderRepository,
            IOrderBuilder orderBuilder,
            ILogger<CreateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _orderBuilder = orderBuilder;
            _logger = logger;
        }

        public async Task<ErrorOr<CreateOrderOutputModel>> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateOrderCommand for Buyer {BuyerId}", request.BuyerId);

            var orderResult = _orderBuilder
                .WithBuyer(request.BuyerId)
                .Build();

            if (orderResult.IsError)
            {
                _logger.LogWarning(
                    "OrderBuilder failed for Buyer {BuyerId}: {ErrorCode}",
                    request.BuyerId,
                    orderResult.FirstError.Code);
                return orderResult.FirstError;
            }

            var order = orderResult.Value;

            try
            {
                await _orderRepository.AddAsync(order, cancellationToken);
                _logger.LogInformation("Order {OrderId} created successfully for Buyer {BuyerId}.", order.Id, request.BuyerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Order for Buyer {BuyerId}.", request.BuyerId);
                return OrderError.UnexpectedError;
            }

            return new CreateOrderOutputModel(order.Id, order.BuyerId, order.Status);
        }
    }
}
