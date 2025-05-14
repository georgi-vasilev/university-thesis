namespace Application.Order.Commands.Create
{
    using ErrorOr;
    using MediatR;

    public record CreateOrderCommand : IRequest<ErrorOr<CreateOrderOutputModel>>;
}
