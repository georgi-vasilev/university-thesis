namespace Application.Events.Commands.Create
{
    using Domain.Event;

    public record CreateEventOutputModel
    {
        public CreateEventOutputModel(
            string name,
            string desription,
            DateOnly date,
            TimeRange time)
        {
            Name = name;
            Description = desription;
            Date = date;
            Time = time;
        }

        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public DateOnly Date { get; init; }
        public TimeRange Time { get; init; } = default!;
    }
}
