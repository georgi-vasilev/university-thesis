namespace Domain.Order.Error
{
    using ErrorOr;

    internal static class OrderError
    {
        public static readonly Error TicketNotFoundError = Error.Validation(
            code: "Order.Ticket",
            description: "Ticket not found.");

        public static readonly Error NoTicketsInOrderError = Error.Validation(
            code: "Order.Ticket",
            description: "No tickets found in the order.");

        public static readonly Error InvalidBuyerError = Error.Validation(
            code: "Order.Buyer",
            description: "Buyer Id cannot be null or empty.");

        public static readonly Error InvalidOrderStatusChangeOperationError = Error.Validation(
            code: "Order.StatusChange",
            description: "Invalid status. The new status is invalid.");

        public static readonly Error CannotChangeOrderStatusError = Error.Validation(
            code: "Order.Buyer",
            description: "Order is cancelled. Status cannot be changed.");
    }
}
