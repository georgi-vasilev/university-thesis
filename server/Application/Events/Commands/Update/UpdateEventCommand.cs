namespace Application.Events.Commands.Update
{
    using ErrorOr;
    using MediatR;

    public record UpdateEventCommand : IRequest<ErrorOr<UpdateCommandOutputModel>>
    {
        public Guid EventId { get; init; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public DateOnly Date { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public Guid VenueId { get; init; }
    }
}
