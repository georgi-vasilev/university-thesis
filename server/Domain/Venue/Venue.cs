namespace Domain.Venue
{
    using Common;
    using Event;
    using System.Collections.Generic;

    public class Venue : IAggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public int Capacity { get; private set; }
        public VenueType Type { get; private set; }


        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

        internal Venue(Guid id, string name, Address address, int capacity, VenueType type)
        {
            Id = id;
            Name = name;
            Address = address;
            Capacity = capacity;
            Type = type;
        }

        private void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        private void ClearDomainEvents() => _domainEvents.Clear();
    }
}
