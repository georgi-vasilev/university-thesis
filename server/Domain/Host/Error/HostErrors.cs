namespace Domain.Host.Error
{
    using ErrorOr;

    public static class HostErrors
    {
        public static readonly Error EventAlreadyAddedError = Error.Validation(
           code: "Host.EventExists",
           description: "The event already exists.");

        public static readonly Error NullLocationError = Error.Validation(
            code: "Host.Location",
            description: "Location cannot be null.");

        public static readonly Error EventDoesNotExistError = Error.Validation(
            code: "Host.Event",
            description: "Event does not exist!");

        public static readonly Error InvalidVenueError = Error.Validation(
            code: "Host.Venue",
            description: "Venue Id cannot be null.");

        public static readonly Error HostNotFoundError = Error.Validation(
            code: "Host.NotFound",
            description: "Host not found.");

        public static readonly Error UnexpectedError = Error.Validation(
            code: "Host.CreateHostCommand",
            description: "Unexpected error occurred");
    }
}
