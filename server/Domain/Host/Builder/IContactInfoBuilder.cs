namespace Domain.Host.Builder
{
    using ErrorOr;

    public interface IContactInfoBuilder
    {
        IContactInfoBuilder WithId(Guid id);
        IContactInfoBuilder WithFirstName(string firstName);
        IContactInfoBuilder WithLastName(string lastName);
        IContactInfoBuilder WithPhoneNumber(string phoneNumber);
        IContactInfoBuilder WithEmail(string emailAddress);
        IContactInfoBuilder WithInstagramHandler(string instagramHandler);
        ErrorOr<ContactInfo> Build();
    }
}
