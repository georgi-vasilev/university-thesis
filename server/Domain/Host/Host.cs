namespace Domain.Host
{
    using Common;
    using Error;
    using ErrorOr;

    public class Host : IAggregateRoot
    {
        private readonly HashSet<Guid> _organizedEventIds = new HashSet<Guid>();
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;
        public Guid Id { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public Guid VenueId { get; private set; }
        public IReadOnlyCollection<Guid> OrganizedEventIds => _organizedEventIds;

        internal Host(
            ContactInfo contactInfo,
            Guid? venueId = null,
            Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            ContactInfo = contactInfo;
            VenueId = venueId ?? Guid.Empty;
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

        public ErrorOr<Success> UpdatePhoneNumber(string phoneNumber)
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

        private void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        private void ClearDomainEvents() => _domainEvents.Clear();
    }
}
