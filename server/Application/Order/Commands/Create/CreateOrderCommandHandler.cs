namespace Application.Order.Commands.Create
{
    using Domain.Order.Builder;
    using Domain.Order.Repository;
    using ErrorOr;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ErrorOr<CreateOrderOutputModel>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderBuilder _orderBuilder;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IOrderBuilder orderBuilder)
        {
            _orderRepository = orderRepository;
            _orderBuilder = orderBuilder;
        }

        public async Task<ErrorOr<CreateOrderOutputModel>> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            var orderResult = _orderBuilder
                .WithBuyer(request.BuyerId)
                .Build();

            if (orderResult.IsError)
            {
                return orderResult.FirstError;
            }

            var order = orderResult.Value;

            await _orderRepository.AddAsync(order, cancellationToken);

            return new CreateOrderOutputModel(order.Id, order.BuyerId, order.Status);
        }
    }
}
