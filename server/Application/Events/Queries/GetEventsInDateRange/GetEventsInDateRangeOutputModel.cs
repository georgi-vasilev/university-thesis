namespace Application.Events.Queries.GetEventsInDateRange
{
    using Domain.Event;

    public record GetEventsInDateRangeOutputModel
    {
        public GetEventsInDateRangeOutputModel(
         Guid id,
         string name,
         string description,
         DateOnly date,
         TimeRange time,
         string status)
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
        public string Status { get; init; }
    }
}
