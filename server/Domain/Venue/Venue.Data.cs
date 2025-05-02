namespace Domain.Venue
{
    using Domain.Common;
    using Domain.Event;

    internal class VenueData : IInitialData
    {
        public Type EntityType => typeof(Venue);

        public IEnumerable<object> GetData()
            => new List<Venue>
            {
                new Venue(
                    id: Guid.NewGuid(),
                    name: "Mixtape",
                    address: new Address("bulevard Bulgaria", "Sofia", "Sofia","Bulgaria", "1700"),
                    capacity: 250,
                    type: VenueType.Club),
                new Venue(
                    id: Guid.NewGuid(),
                    name: "BNKR",
                    address: new Address("bulevard Bulgaria", "Sofia", "Sofia", "Bulgaria", "1700"),
                    capacity: 150,
                    type: VenueType.Club),
                new Venue(
                    id: Guid.NewGuid(),
                    name: "KUPE",
                    address: new Address("bulevard Patriarh Evtimii", "Sofia", "Sofia","Bulgaria", "1700"),
                    capacity: 50,
                    type: VenueType.Club),
                new Venue(
                    id: Guid.NewGuid(),
                    name: "SODA",
                    address: new Address("bulevard Bulgaria", "Sofia", "Sofia","Bulgaria", "1700"),
                    capacity: 50,
                    type: VenueType.Bar),
            };
    }
}
