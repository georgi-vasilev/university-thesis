namespace Domain.Ticket
{
    internal class Ticket
    {
        private readonly Guid _id;
        private readonly Guid _eventId;

        public Guid Id { get; private set; }
        public Guid EventId { get; private set; }
        public Price Price { get; private set; }
        public string Curreny { get; private set; } 
    }
}
