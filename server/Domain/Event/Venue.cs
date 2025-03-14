namespace Domain.Event
{
    internal record Venue
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public int Capacity { get; private set; }
        public VenueType Type { get; private set; }

        internal Venue(Guid id, string name, Address address, int capacity, VenueType type)
        {
            Id = id;
            Name = name;
            Address = address;
            Capacity = capacity;
            Type = type;
        }
    }
}
