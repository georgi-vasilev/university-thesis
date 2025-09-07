namespace Domain.Event.Builder
{
    using Domain.Common;

    public interface IEventBuilder : IBuilder<Event>
    {
        IEventBuilder WithName(string name);
        IEventBuilder WithDescription(string description);
        IEventBuilder WithImageUrl(string imageUrl);
        IEventBuilder WithDate(DateOnly date);
        IEventBuilder WithTime(TimeRange time);
        IEventBuilder WithVenue(Guid venueId);
        IEventBuilder WithHostId(Guid hostId);
        IEventBuilder WithId(Guid id);
        IEventBuilder WithGeneralTicketPrice(decimal price);
        IEventBuilder WithVipTicketPrice(decimal? price);
    }
}
