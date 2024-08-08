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

        public static readonly Error InvalidDateValueError = Error.Validation(
            code: "Event.InvalidDate",
            description: "Invalid date!");

        public static readonly Error InvalidOrganizerIdValueError = Error.Validation(
            code: "Event.InvalidOrganizerId",
            description: "OrganizerId cannot be default");

        public static readonly Error InvalidStatusChangeOperationFromCancelledToActive = Error.Validation(
            code: "Event.InvalidStatusChange",
            description: "Cannot change status from Cancelled to Active.");

        public static readonly Error InvalidStatusChangeOperationFromCancelledToPostponed = Error.Validation(
            code: "Event.InvalidStatusChange",
            description: "Cannot change status from  Cancelled to Postponed.");
    }
}
