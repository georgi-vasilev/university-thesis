using Domain.Event;

namespace Application.Events.Queries.GetEvents
{
    public record GetEventsOutputModel
    {
        public GetEventsOutputModel(
            Guid id,
            string name,
            string description,
            DateOnly date,
            TimeRange time,
            EventStatus status)
        {
            Id = id;
            Name = name;
            Description = description;
            Date = date;
            Time = time;
            Status = status;

        }

        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public DateOnly Date { get; init; }
        public TimeRange Time { get; init; } = default!;
        public EventStatus Status { get; init; }
    }
}
