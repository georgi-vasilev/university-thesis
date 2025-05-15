namespace Domain.Common
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
