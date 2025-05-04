namespace Domain.Host
{
    using Common;
    using Error;
    using ErrorOr;

    public class Host : IAggregateRoot
    {
        private readonly List<Guid> _organizedEventIds = new List<Guid>();
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;
        public Guid Id { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public Guid? VenueId { get; private set; }
        public IReadOnlyList<Guid> OrganizedEventIds => _organizedEventIds;

        private Host()
        {
            
        }

        internal Host(
            ContactInfo contactInfo,
            Guid? venueId = null,
            Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            ContactInfo = contactInfo;
            VenueId = venueId;
        }


        public ErrorOr<Success> AddOrganizedEvent(Guid eventId)
        {
            var exists = _organizedEventIds.Any(x => x == eventId);
            if (exists)
            {
                return HostErrors.EventAlreadyAddedError;
            }

            _organizedEventIds.Add(eventId);

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

        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
