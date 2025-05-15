namespace Application.Events.Commands.Cancel
{
    using ErrorOr;
    using MediatR;

    public record CancelEventCommand(Guid Id) : IRequest<ErrorOr<Success>>;
}
