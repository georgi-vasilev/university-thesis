namespace Domain.Event.Error
{
    using ErrorOr;
    using System.Data;

    public static class EventErrors
    {
        public static readonly Error NullTimeError = Error.Validation(
            code: "Event.InvalidTime",
            description: "Time cannot be null");

        public static readonly Error NullVenueError = Error.Validation(
            code: "Event.InvalidVenue",
            description: "Venue cannot be null");

        public static readonly Error InvalidEndTimeError = Error.Validation(
            code: "Event.InvalidTime",
            description: "End time must be after start time");

        public static readonly Error DefaultDateValueError = Error.Validation(
            code: "Event.InvalidDate",
            description: "Date cannot be default");

        public static readonly Error DateIsInThePastError = Error.Validation(
            code: "Event.InvalidDate",
            description: "The new date cannot be in the past!");

        public static readonly Error InvalidOrganizerIdValueError = Error.Validation(
            code: "Event.InvalidHostId",
            description: "Host cannot be default");

        public static readonly Error InvalidStatusChangeOperationFromCancelledToActiveError = Error.Validation(
            code: "Event.InvalidStatusChange",
            description: "Cannot change status from Cancelled to Active.");

        public static readonly Error InvalidStatusChangeOperationFromCancelledToPostponedError = Error.Validation(
            code: "Event.InvalidStatusChange",
            description: "Cannot change status from  Cancelled to Postponed.");

        public static readonly Error CapacityExceededError = Error.Validation(
            code: "Event.CapacityExceeded",
            description: "Ticket sale exceeds venue capacity.");

        public static readonly Error InvalidCapacityError = Error.Validation(
            code: "Event.InvalidCapacity",
            description: "Invalid capacity amount.");

        public static readonly Error TicketAlreadyAddedError = Error.Validation(
            code: "Event.TicketAdded",
            description: "Ticket already added.");

        public static readonly Error InvalidDescriptionError = Error.Validation(
            code: "Event.InvalidDescription",
            description: "Description cannot be null or empty.");

        public static readonly Error InvalidNameError = Error.Validation(
            code: "Event.InvalidName",
            description: "Name cannot be null or empty.");

        public static readonly Error EventNotFoundError = Error.Validation(
            code: "Event.Update",
            description: "Event not found");

        public static readonly Error EventDoesNotBelongToHostError = Error.Validation(
            code: "Event.InvalidHost",
            description: "The provided host does not match the event's host.");

        public static readonly Error NoTicketsLeftError = Error.Validation(
            code: "Event.TicketSale",
            description: "No tickets left for sale.");

        public static readonly Error EventHasEndedOrCancelledError = Error.Validation(
            code: "Event.TicketSale",
            description: "Event has ended or was cancelled.");

        public static readonly Error CannotDeleteEventWithOrders = Error.Validation(
            code: "Event.Cancellation",
            description: "Cannot cancell event with sold tickets.");

        public static readonly Error OverlappingEventError = Error.Validation(
            code: "Event.Overlapping",
            description: "Event overlaps with a different one.");
    }
}
