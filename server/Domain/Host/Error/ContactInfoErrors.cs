namespace Domain.Host.Error
{
    using ErrorOr;

    public static class ContactInfoErrors
    {
        public static readonly Error InvalidContactInfoError = Error.Validation(
            code: "Host.ContactInfo",
            description: "Contact info cannot be null.");

        public static readonly Error InvalidPhoneNumberError = Error.Validation(
            code: "Host.ContactInfo",
            description: "Phone number cannot be empty or null.");

        public static readonly Error InvalidNameError = Error.Validation(
            code: "Host.ContactInfo",
            description: "Name cannot be null or empty.");

        public static readonly Error EmailNullOrEmptyError = Error.Validation(
           code: "Host.ContactInfo",
           description: "Email cannot be null or emtpy.");

        public static readonly Error InvalidEmailError = Error.Validation(
            code: "Host.ContactInfo",
            description: "Email is invalid.");

        public static readonly Error InstagramHandlerNullOrEmptyError = Error.Validation(
            code: "Host.ContactInfo",
            description: "Instagram handler cannot be null or emtpy.");
    }
}
