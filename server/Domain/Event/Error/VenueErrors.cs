namespace Domain.Event.Error
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
    }
}
