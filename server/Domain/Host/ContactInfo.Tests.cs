namespace Domain.Host
{
    using Factory;
    using FluentAssertions;
    using Xunit;

    public class ContactInfoTests
    {
        [Fact]
        public void Can_Create_Valid_Contact_Info()
        {
            var contactInfo = new ContactInfoBuilder()
                .WithId(Guid.NewGuid())
                .WithFirstName("Petar")
                .WithLastName("Petrov")
                .WithPhoneNumber("+359823402823")
                .WithEmail("pesho@petrov.bg")
                .WithInstagramHandler("@pesho")
                .Build();

            contactInfo.IsError.Should().BeFalse();
        }
    }
}
