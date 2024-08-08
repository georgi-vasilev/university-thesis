namespace Domain.Event.Factory
{
    using Domain.Event.Error;
    using ErrorOr;

    internal class AddressBuilder
    {
        private string _street;
        private string _city;
        private string _state;
        private string _country;
        private string _zipCode;

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
