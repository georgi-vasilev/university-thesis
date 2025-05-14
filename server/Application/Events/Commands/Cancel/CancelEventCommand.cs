namespace Application.Events.Commands.Cancel
{
    using Domain.Event;
    using ErrorOr;
    using MediatR;

    public record CancelEventCommand(Guid EventId, EventStatus Status) : IRequest<ErrorOr<Success>>;
}
