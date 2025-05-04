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
            string host,
            DateOnly date,
            TimeRange time,
            string status,
            int availableTickets)
        {
            Id = id;
            Name = name;
            Description = description;
            Venue = venue;
            Host = host;
            Date = date;
            Time = time;
            Status = status;
            AvailableTickets = availableTickets;
        }
        public Guid Id { get; set; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public string Venue { get; set; } = default!;
        public string Host { get; set; } = default!;
        public DateOnly Date { get; init; }
        public TimeRange Time { get; init; } = default!;
        public string Status { get; set; }
        public int AvailableTickets { get; set; }
    }
}
