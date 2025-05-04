namespace Application.Order.Commands.Create
{
    using Domain.Event.Error;
    using Domain.Order.Builder;
    using Domain.Order.Error;
    using Domain.Order.Repository;
    using ErrorOr;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Services.Contracts.User;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ErrorOr<CreateOrderOutputModel>>
    {
        private readonly IOrderDomainRepository _orderRepository;
        private readonly IOrderBuilder _orderBuilder;
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly ICurrentUser _currentUser;

        public CreateOrderCommandHandler(
            IOrderDomainRepository orderRepository,
            IOrderBuilder orderBuilder,
            ILogger<CreateOrderCommandHandler> logger,
            ICurrentUser currentUser)
        {
            _orderRepository = orderRepository;
            _orderBuilder = orderBuilder;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<CreateOrderOutputModel>> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUser.BuyerId is null)
            {
                _logger.LogWarning("HostId claim is missing for user {UserId}", _currentUser.UserId);
                return EventErrors.Unauthorized;
            }

            var buyerId = _currentUser.BuyerId.Value;
            _logger.LogInformation("Handling CreateOrderCommand for Buyer {BuyerId}", buyerId);

            var orderResult = _orderBuilder
                .WithBuyer(buyerId)
                .Build();

            if (orderResult.IsError)
            {
                _logger.LogWarning(
                    "OrderBuilder failed for Buyer {BuyerId}: {ErrorCode}",
                    buyerId,
                    orderResult.FirstError.Code);
                return orderResult.FirstError;
            }

            var order = orderResult.Value;

            try
            {
                await _orderRepository.AddAsync(order, cancellationToken);
                _logger.LogInformation("Order {OrderId} created successfully for Buyer {BuyerId}.", order.Id, buyerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Order for Buyer {BuyerId}.", buyerId);
                return OrderError.UnexpectedError;
            }

            return new CreateOrderOutputModel(order.Id, order.BuyerId, order.Status);
        }
    }
}
