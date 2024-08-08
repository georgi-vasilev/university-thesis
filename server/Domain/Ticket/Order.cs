namespace Domain.Ticket
{
    internal class Order
    {
        private readonly Guid _id;
        private readonly Guid _userId;
        private readonly HashSet<Ticket> _tickets;
    }
}
