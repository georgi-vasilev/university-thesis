namespace Domain.Host.Builder
{
    using Error;
    using ErrorOr;
    using System.ComponentModel.DataAnnotations;

    internal class ContactInfoBuilder : IContactInfoBuilder
    {
        private string _firstName = default!;
        private string _lastName = default!;
        private string _phoneNumber = default!;
        private string _email = default!;
        private string _instagramHandler = default!;
        private Guid _id = Guid.NewGuid();

        public IContactInfoBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public IContactInfoBuilder WithFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        public IContactInfoBuilder WithLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        public IContactInfoBuilder WithPhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            return this;
        }

        public IContactInfoBuilder WithEmail(string emailAddress)
        {
            _email = emailAddress;
            return this;
        }

        public IContactInfoBuilder WithInstagramHandler(string instagramHandler)
        {
            _instagramHandler = instagramHandler;
            return this;
        }

        public ErrorOr<ContactInfo> Build()
        {
            if (string.IsNullOrWhiteSpace(_firstName))
            {
                return ContactInfoErrors.InvalidNameError;
            }

            if (string.IsNullOrWhiteSpace(_lastName))
            {
                return ContactInfoErrors.InvalidNameError;
            }

            if (string.IsNullOrEmpty(_phoneNumber))
            {
                return ContactInfoErrors.InvalidPhoneNumberError;
            }

            if (string.IsNullOrEmpty(_email))
            {
                return ContactInfoErrors.EmailNullOrEmptyError;
            }

            var email = new EmailAddressAttribute();
            if (!email.IsValid(_email))
            {
                return ContactInfoErrors.InvalidEmailError;
            }

            if (string.IsNullOrEmpty(_instagramHandler))
            {
                return ContactInfoErrors.InstagramHandlerNullOrEmptyError;
            }


            return new ContactInfo(_id, _firstName, _lastName, _phoneNumber, _email, _instagramHandler);
        }
    }
}
