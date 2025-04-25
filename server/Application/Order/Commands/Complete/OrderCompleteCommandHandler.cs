namespace Application.Order.Commands.Complete
{
    using Domain.Order.Error;
    using Domain.Order.Repository;
    using ErrorOr;
    using MediatR;

    public class OrderCompleteCommandHandler : IRequestHandler<OrderCompleteCommand, ErrorOr<OrderCompleteOutputModel>>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderCompleteCommandHandler(IOrderRepository orderRepository) 
            => _orderRepository = orderRepository;

        public async Task<ErrorOr<OrderCompleteOutputModel>> Handle(OrderCompleteCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
            {
                return OrderError.OrderNotFoundError;
            }

            var result = order.CompleteOrder();
            if (result.IsError)
            {
                return result.FirstError;
            }

            await _orderRepository.UpdateAsync(order, cancellationToken);

            return new OrderCompleteOutputModel(order.Id, order.Status);
        }
    }
}
