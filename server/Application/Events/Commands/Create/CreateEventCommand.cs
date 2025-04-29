namespace Application.Events.Commands.Create
{
    using ErrorOr;
    using MediatR;

    public record CreateEventCommand : IRequest<ErrorOr<CreateEventOutputModel>>
    {
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public DateOnly Date { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public Guid VenueId { get; init; }
        public Guid HostId { get; init; }
        public int Capacity { get; init; }
    }
}
