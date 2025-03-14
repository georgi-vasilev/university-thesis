namespace Domain.Host
{
    using Error;
    using ErrorOr;
    using System.ComponentModel.DataAnnotations;

    internal record ContactInfo
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }
        public string PhoneNumber { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string InstagramHandler { get; private set;} = default!;

        internal ContactInfo(
            Guid id,
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            string instagramHandler)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            InstagramHandler = instagramHandler;
        }

        public ErrorOr<ContactInfo> UpdatePhoneNumber(string newPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(newPhoneNumber))
            {
                return ContactInfoErrors.InvalidPhoneNumberError;
            }

            return this with { PhoneNumber = newPhoneNumber };
        }

        public ErrorOr<ContactInfo> UpdateEmailAddress(string newEmailAddress)
        {
            if (string.IsNullOrEmpty(newEmailAddress))
            {
                return ContactInfoErrors.EmailNullOrEmptyError;
            }

            var email = new EmailAddressAttribute();
            if (!email.IsValid(newEmailAddress))
            {
                return ContactInfoErrors.InvalidEmailError;
            }

            return this with { Email = newEmailAddress };
        }

        public ErrorOr<ContactInfo> UpdateInstagramHandler(string newInstagramHandler)
        {
            if (string.IsNullOrEmpty(newInstagramHandler))
            {
                return ContactInfoErrors.InstagramHandlerNullOrEmptyError;
            }

            return this with { InstagramHandler = newInstagramHandler };
        }
    }
}
