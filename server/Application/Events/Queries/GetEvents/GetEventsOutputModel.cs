namespace Application.Events.Queries.GetEvents
{
    using Domain.Event;

    public record GetEventsOutputModel
    {
        public GetEventsOutputModel(
            Guid id,
            string name,
            string description,
            string imageUrl,
            DateOnly date,
            TimeRange time,
            string status)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Date = date;
            Time = time;
            Status = status;
        }

        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public string ImageUrl { get; set; } = default!;
        public DateOnly Date { get; init; }
        public TimeRange Time { get; init; } = default!;
        public string Status { get; init; }
    }
}
