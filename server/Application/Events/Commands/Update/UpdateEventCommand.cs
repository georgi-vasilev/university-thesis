namespace Application.Events.Commands.Update
{
    using Domain.Event;
    using ErrorOr;
    using MediatR;

    public record UpdateEventCommand : IRequest<ErrorOr<UpdateCommandOutputModel>>
    {
        public Guid EventId { get; init; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public DateOnly Date { get; init; }
        public TimeRange Time { get; init; } = default!;
        public Guid VenueId { get; init; }
    }
}
