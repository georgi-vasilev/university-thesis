namespace Application.Events.Commands.Cancel
{
    using Domain.Event;
    using ErrorOr;
    using MediatR;

    public record CancelEventCommand : IRequest<ErrorOr<Success>>
    {
        public Guid HostId { get; init; }
        public Guid EventId { get; init; }
        public EventStatus Status { get; init; }
    }
}
