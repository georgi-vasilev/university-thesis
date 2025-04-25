namespace Application.Events.Commands.Cancel
{
    using Domain.Event;
    using ErrorOr;
    using MediatR;

    public record CancelEventCommand : IRequest<ErrorOr<Success>>
    {
        public Guid HostId { get; set; }
        public Guid EventId { get; set; }
        public EventStatus Status { get; set; }
    }
}
