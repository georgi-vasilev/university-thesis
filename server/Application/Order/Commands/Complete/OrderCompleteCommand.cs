namespace Application.Order.Commands.Complete
{
    using ErrorOr;
    using MediatR;

    public record OrderCompleteCommand : IRequest<ErrorOr<OrderCompleteOutputModel>>
    {
        public Guid OrderId { get; set; }
    }
}
