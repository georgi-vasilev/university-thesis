namespace Domain.Venue.Builder
{
    using Event;
    using Error;
    using ErrorOr;

    internal class VenueBuilder : IVenueBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _name = default!;
        private Address _address = default!;
        private int _capacity;
        private VenueType _type;

        public IVenueBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public IVenueBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public IVenueBuilder WithAddress(Address address)
        {
            _address = address;
            return this;
        }

        public IVenueBuilder WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public IVenueBuilder WithType(VenueType type)
        {
            _type = type;
            return this;
        }

        public ErrorOr<Venue> Build()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                return VenueErrors.EmptyNameError;
            }

            if (_address is null)
            {
                return VenueErrors.NullAddressError;
            }

            if (_capacity <= 0)
            {
                return VenueErrors.CapacityLessThanZeroError;
            }

            return new Venue(_id, _name, _address, _capacity, _type);
        }
    }
}
