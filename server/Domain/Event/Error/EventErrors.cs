namespace Domain.Event.Error
{
    using ErrorOr;

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

        public static readonly Error InvalidStatusChangeOperationFromCancelledToActive = Error.Validation(
            code: "Event.InvalidStatusChange",
            description: "Cannot change status from Cancelled to Active.");

        public static readonly Error InvalidStatusChangeOperationFromCancelledToPostponed = Error.Validation(
            code: "Event.InvalidStatusChange",
            description: "Cannot change status from  Cancelled to Postponed.");

        public static readonly Error CapacityExceeded = Error.Validation(
            code: "Event.CapacityExceeded",
            description: "Ticket sale exceeds venue capacity.");

        public static readonly Error TicketAlreadyAdded = Error.Validation(
            code: "Event.TicketAdded",
            description: "Ticket already added.");

        public static readonly Error InvalidDescription = Error.Validation(
            code: "Event.InvalidDescription",
            description: "Description cannot be null or empty.");

        public static readonly Error InvalidName = Error.Validation(
            code: "Event.InvalidName",
            description: "Name cannot be null or empty.");
    }
}
