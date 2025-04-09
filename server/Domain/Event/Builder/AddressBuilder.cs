namespace Domain.Event.Builder
{
    using Error;
    using ErrorOr;

    internal class AddressBuilder
    {
        private string _street = default!;
        private string _city = default!;
        private string _state = default!;
        private string _country = default!;
        private string _zipCode = default!;

        public AddressBuilder WithStreet(string street)
        {
            _street = street;
            return this;
        }

        public AddressBuilder WithCity(string city)
        {
            _city = city;
            return this;
        }

        public ErrorOr<Address> Build()
        {
            if (string.IsNullOrWhiteSpace(_street))
            {
                return AddressErrors.EmptyStreetError;
            }

            if (string.IsNullOrWhiteSpace(_city))
            {
                return AddressErrors.EmptyCityErorr;
            }

            return new Address(_street, _city, _state, _country, _zipCode);
        }
    }
}
