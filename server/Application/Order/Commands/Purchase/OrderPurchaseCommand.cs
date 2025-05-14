namespace Application.Order.Commands.Purchase
{
    using Domain.Common.ValueObject;
    using Domain.Order;
    using ErrorOr;
    using MediatR;

    public record OrderPurchaseCommand(Guid EventId, Money PaymentAmount, TicketType TicketType) : IRequest<ErrorOr<Success>>;
}
