namespace Application.Events.Queries.GetEventDetails
{
    using Domain.Event;

    public class GetEventDetailsOutputModel
    {
        public GetEventDetailsOutputModel(
            Guid id,
            string name,
            string description,
            string imageUrl,
            string venue,
            string hostInstagramHandler,
            DateOnly date,
            TimeRange time,
            decimal generalPrice,
            decimal? vipPrice,
            string status,
            int availableTickets)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Venue = venue;
            HostInstagramHandler = hostInstagramHandler;
            Date = date;
            Time = time;
            GeneralPrice = generalPrice;
            VipPrice = vipPrice;
            Status = status;
            AvailableTickets = availableTickets;
        }
        public Guid Id { get; set; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public string ImageUrl { get; init; } = default!;
        public string Venue { get; set; } = default!;
        public string HostInstagramHandler { get; set; } = default!;
        public DateOnly Date { get; init; }
        public TimeRange Time { get; init; } = default!;
        public decimal GeneralPrice { get; init; }
        public decimal? VipPrice { get; init; }
        public string Status { get; set; }
        public int AvailableTickets { get; set; }
    }
}
