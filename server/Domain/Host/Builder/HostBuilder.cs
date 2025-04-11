namespace Domain.Host.Builder
{
    using Error;
    using ErrorOr;

    internal class HostBuilder : IHostBuilder
    {
        private ContactInfo _contactInfo = default!;
        private Guid _venueId = default!;
        private Guid? _id;

        public IHostBuilder WithContactInfo(ContactInfo contactInfo)
        {
            _contactInfo = contactInfo;
            return this;
        }

        public IHostBuilder WithVenueId(Guid venueId)
        {
            _venueId = venueId;
            return this;
        }

        public IHostBuilder WithId(Guid id)
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

            return new Host(_contactInfo, _venueId, _id);
        }
    }
}
