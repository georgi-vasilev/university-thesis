namespace Domain.Order.Error
{
    using ErrorOr;

    public static class TicketError
    {
        public static readonly Error AlreadySoldError = Error.Validation(
            code: "Tickets.SoldOut",
            description: "Tickets already sold out.");

        public static readonly Error InvalidEventError = Error.Validation(
            code: "Tickets.Event",
            description: "No tickets for this event.");
    }
}
