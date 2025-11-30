namespace Domain.Buyer
{
    using Common;
    using System.Collections.Generic;

    public class Buyer : IAggregateRoot
    {
        private readonly HashSet<Guid> _orderIds = new HashSet<Guid>();
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }
        public IReadOnlyCollection<Guid> OrderIds { get => _orderIds; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

        private Buyer() { }

        internal Buyer(string firstName, string lastName, string email, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
