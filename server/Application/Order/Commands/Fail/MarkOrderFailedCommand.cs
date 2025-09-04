namespace Application.Order.Commands.Fail
{
    using ErrorOr;
    using MediatR;

    public record MarkOrderFailedCommand(
        string TransactionId,
        string FailureReason) : IRequest<ErrorOr<Success>>;
}
