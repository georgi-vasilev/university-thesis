namespace Domain.Event.Builder
{
    using Domain.Common;

    public interface IEventBuilder : IBuilder<Event>
    {
        IEventBuilder WithName(string name);
        IEventBuilder WithDescription(string description);
        IEventBuilder WithDate(DateOnly date);
        IEventBuilder WithTime(TimeRange time);
        IEventBuilder WithVenue(Guid venueId);
        IEventBuilder WithHostId(Guid hostId);
        IEventBuilder WithCapacity(int capacity);
        IEventBuilder WithId(Guid id);
    }
}
