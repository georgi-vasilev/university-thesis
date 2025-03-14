namespace Domain.Host
{
    using Error;
    using ErrorOr;

    internal class Host
    {
        private readonly HashSet<Guid> _organizedEventIds = new HashSet<Guid>();

        public Guid Id { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public Guid VenueId { get; private set; }
        public IReadOnlyCollection<Guid> OrganizedEventIds => _organizedEventIds;

        internal Host(
            ContactInfo contactInfo,
            Guid venueId,
            Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            ContactInfo = contactInfo;
            VenueId = venueId;
        }


        public ErrorOr<Success> AddOrganizedEvent(Guid eventId)
        {
            if (!_organizedEventIds.Add(eventId))
            {
                return HostErrors.EventAlreadyAddedError;
            }

            //TODO: dispatch domain event
            return Result.Success;
        }

        public ErrorOr<Success> UpdateVenue(Guid newVenueId)
        {
            if (newVenueId == Guid.Empty)
            {
                return HostErrors.InvalidVenueError;
            }

            VenueId = newVenueId;

            // TODO: Dispatch a domain event
            return Result.Success;
        }

        public ErrorOr<Success> DeleteOrganizedEvent(Guid eventId)
        {
            if (!_organizedEventIds.Contains(eventId))
            {
                return HostErrors.EventDoesNotExistError;
            }

            _organizedEventIds.Remove(eventId);

            //TODO: dispatch domain event
            return Result.Success;
        }

        public ErrorOr<Success> UpdateContactInfo(string phoneNumber)
        {
            var updatedContactInfo = ContactInfo.UpdatePhoneNumber(phoneNumber);
            if (updatedContactInfo.IsError)
            {
                return updatedContactInfo.FirstError;
            }

            ContactInfo = updatedContactInfo.Value;

            //TODO: dispatch domain event
            return Result.Success;
        }

        public ErrorOr<Success> UpdateEmail(string newEmail)
        {
            var updatedContactInfo = ContactInfo.UpdateEmailAddress(newEmail);
            if (updatedContactInfo.IsError)
            {
                return updatedContactInfo.FirstError;
            }

            ContactInfo = updatedContactInfo.Value;

            //TODO: dispatch domain event
            return Result.Success;
        }

        public ErrorOr<Success> UpdateInstagramHandler(string newInstagramHandler)
        {
            var updatedContactInfo = ContactInfo.UpdateInstagramHandler(newInstagramHandler);
            if (updatedContactInfo.IsError)
            {
                return updatedContactInfo.FirstError;
            }

            ContactInfo = updatedContactInfo.Value;

            //TODO: dispatch domain event
            return Result.Success;
        }
    }
}
