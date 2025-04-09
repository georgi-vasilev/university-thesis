namespace Domain.Venue.Builder
{
    using Common;
    using Event;

    public interface IVenueBuilder : IBuilder<Venue>
    {
        IVenueBuilder WithId(Guid id);
        IVenueBuilder WithName(string name);
        IVenueBuilder WithAddress(Address address);
        IVenueBuilder WithCapacity(int capacity);
        IVenueBuilder WithType(VenueType type);
    }
}
