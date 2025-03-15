namespace Domain.Host.Factory
{
    using Error;
    using ErrorOr;

    internal class HostBuilder
    {
        private ContactInfo _contactInfo = default!;
        private Guid _venueId = default!;
        private Guid? _id;

        public HostBuilder WithContactInfo(ContactInfo contactInfo)
        {
            _contactInfo = contactInfo;
            return this;
        }

        public HostBuilder WithVenueId(Guid venueId)
        {
            _venueId = venueId;
            return this;
        }

        public HostBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ErrorOr<Host> Build()
        {

            if (_contactInfo is null)
            {
                return ContactInfoErrors.InvalidContactInfoError;
            }

            if (_venueId == Guid.Empty)
            {
                return HostErrors.NullLocationError;
            }

            return new Host(_contactInfo, _venueId, _id);
        }
    }
}
