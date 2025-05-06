namespace Application.Events.Queries.GetEventDetails
{
    using Domain.Event;

    public class GetEventDetailsOutputModel
    {
        public GetEventDetailsOutputModel(
            Guid id,
            string name,
            string description,
            string venue,
            string hostInstagramHandler,
            DateOnly date,
            TimeRange time,
            string status,
            int availableTickets)
        {
            Id = id;
            Name = name;
            Description = description;
            Venue = venue;
            HostInstagramHandler = hostInstagramHandler;
            Date = date;
            Time = time;
            Status = status;
            AvailableTickets = availableTickets;
        }
        public Guid Id { get; set; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public string Venue { get; set; } = default!;
        public string HostInstagramHandler { get; set; } = default!;
        public DateOnly Date { get; init; }
        public TimeRange Time { get; init; } = default!;
        public string Status { get; set; }
        public int AvailableTickets { get; set; }
    }
}
