namespace Application.Events.Commands.Update
{
    using Domain.Event;

    public record UpdateCommandOutputModel
    {
        public UpdateCommandOutputModel(
            string name,
            string description, 
            DateOnly date, 
            TimeRange time, 
            string venueName)
        {
            Name = name;
            Description = description;
            Date = date;
            Time = time;
            VenueName = venueName;
        }

        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public DateOnly Date { get; init; }
        public TimeRange Time { get; init; } = default!;
        public string VenueName { get; init; } = default!;
    }
}
