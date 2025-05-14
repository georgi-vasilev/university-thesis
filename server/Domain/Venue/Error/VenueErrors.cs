namespace Domain.Venue.Error
{
    using ErrorOr;

    public static class VenueErrors
    {
        public static readonly Error EmptyNameError = Error.Validation(
            code: "Venue.InvalidName",
            description: "Name cannot be empty");

        public static readonly Error NullAddressError = Error.Validation(
            code: "Venue.NullAddressValue",
            description: "Address cannot be null");

        public static readonly Error CapacityLessThanZeroError = Error.Validation(
            code: "Venue.InvalidCapacity",
            description: "Capacity must be greater than zero");

        public static readonly Error VenueNotFoundError = Error.Validation(
            code: "Venue.NotFound",
            description: "Venue not found!");

        public static readonly Error CapacityExceededError = Error.Validation(
            code: "Venue.CreateEventCommandHandler",
            description: "The requested capacity exceeds the venue capacity.");
    }
}
