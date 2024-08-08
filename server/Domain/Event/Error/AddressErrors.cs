namespace Domain.Event.Error
{

    using ErrorOr;

    public static class AddressErrors
    {
        public static readonly Error EmptyStreetError = Error.Validation(
            code: "Address.InvalidValue",
            description: "Street cannot be empty");

        public static readonly Error EmptyCityErorr = Error.Validation(
            code: "Address.InvalidValue",
            description: "City cannot be empty.");
    }
}
