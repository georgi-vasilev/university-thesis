namespace Domain.Event.Factory
{
    using Domain.Event.Error;
    using ErrorOr;

    internal class VenueBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _name;
        private Address _address;
        private int _capacity;
        private VenueType _type;

        public VenueBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public VenueBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public VenueBuilder WithAddress(Address address)
        {
            _address = address;
            return this;
        }

        public VenueBuilder WithCapacity(int capacity)
        {
            _capacity = capacity;
            return this;
        }

        public VenueBuilder WithType(VenueType type)
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
