namespace Domain.Host
{
    using Builder;
    using Error;
    using FluentAssertions;
    using Xunit;

    public class HostTests
    {
        private Host BuildValidHost()
        {
            var contactInfoResult = new ContactInfoBuilder()
                .WithFirstName("John")
                .WithLastName("Doe")
                .WithPhoneNumber("111-222-3333")
                .WithEmail("john.doe@example.com")
                .WithInstagramHandler("john_doe")
                .Build();

            contactInfoResult.IsError.Should().BeFalse();
            var contactInfo = contactInfoResult.Value;

            var venueId = Guid.NewGuid();

            return new Host(contactInfo, venueId);
        }

        [Fact]
        public void Can_Create_Valid_Host()
        {
            var host = BuildValidHost();

            host.Should().NotBeNull();
            host.Id.Should().NotBeEmpty();
            host.ContactInfo.Should().NotBeNull();
            host.VenueId.Should().NotBeEmpty();
        }

        [Fact]
        public void AddOrganizedEvent_Succeeds_When_EventIdIsNew()
        {
            var host = BuildValidHost();
            var eventId = Guid.NewGuid();

            var result = host.AddOrganizedEvent(eventId);

            result.IsError.Should().BeFalse();
            host.OrganizedEventIds.Should().Contain(eventId);
        }

        [Fact]
        public void AddOrganizedEvent_Fails_When_EventIdAlreadyAdded()
        {
            var host = BuildValidHost();
            var eventId = Guid.NewGuid();
            host.AddOrganizedEvent(eventId);

            var result = host.AddOrganizedEvent(eventId);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(HostErrors.EventAlreadyAddedError);
        }

        [Fact]
        public void DeleteOrganizedEvent_Succeeds_When_EventExists()
        {
            var host = BuildValidHost();
            var eventId = Guid.NewGuid();
            host.AddOrganizedEvent(eventId);

            var result = host.DeleteOrganizedEvent(eventId);

            result.IsError.Should().BeFalse();
            host.OrganizedEventIds.Should().NotContain(eventId);
        }

        [Fact]
        public void DeleteOrganizedEvent_Fails_When_EventDoesNotExist()
        {
            var host = BuildValidHost();
            var eventId = Guid.NewGuid();

            var result = host.DeleteOrganizedEvent(eventId);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(HostErrors.EventDoesNotExistError);
        }

        [Fact]
        public void UpdateContactInfo_Succeeds_When_ValidPhoneNumberProvided()
        {
            var host = BuildValidHost();
            var newPhoneNumber = "999-888-7777";

            var result = host.UpdateContactInfo(newPhoneNumber);

            result.IsError.Should().BeFalse();
            host.ContactInfo.PhoneNumber.Should().Be(newPhoneNumber);
        }

        [Fact]
        public void UpdateContactInfo_Fails_When_InvalidPhoneNumberProvided()
        {
            var host = BuildValidHost();
            var invalidPhoneNumber = "";

            var result = host.UpdateContactInfo(invalidPhoneNumber);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ContactInfoErrors.InvalidPhoneNumberError);
        }

        [Fact]
        public void UpdateEmail_Succeeds_When_ValidEmailProvided()
        {
            var host = BuildValidHost();
            var newEmail = "new.email@example.com";

            var result = host.UpdateEmail(newEmail);

            result.IsError.Should().BeFalse();
            host.ContactInfo.Email.Should().Be(newEmail);
        }

        [Fact]
        public void UpdateEmail_Fails_When_InvalidEmailProvided()
        {
            var host = BuildValidHost();
            var invalidEmail = "invalid-email";

            var result = host.UpdateEmail(invalidEmail);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ContactInfoErrors.InvalidEmailError);
        }

        [Fact]
        public void UpdateInstagramHandler_Succeeds_When_ValidValueProvided()
        {
            var host = BuildValidHost();
            var newInstagramHandler = "new_insta";

            var result = host.UpdateInstagramHandler(newInstagramHandler);

            result.IsError.Should().BeFalse();
            host.ContactInfo.InstagramHandler.Should().Be(newInstagramHandler);
        }

        [Fact]
        public void UpdateInstagramHandler_Fails_When_InvalidValueProvided()
        {
            var host = BuildValidHost();
            var invalidInstagramHandler = "";

            var result = host.UpdateInstagramHandler(invalidInstagramHandler);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(ContactInfoErrors.InstagramHandlerNullOrEmptyError);
        }

        [Fact]
        public void UpdateVenue_Succeeds_When_ValidVenueIdProvided()
        {
            var host = BuildValidHost();
            var newVenueId = Guid.NewGuid();

            var result = host.UpdateVenue(newVenueId);

            result.IsError.Should().BeFalse();
            host.VenueId.Should().Be(newVenueId);
        }

        [Fact]
        public void UpdateVenue_Fails_When_EmptyVenueIdProvided()
        {
            var host = BuildValidHost();

            var result = host.UpdateVenue(Guid.Empty);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(HostErrors.InvalidVenueError);
        }
    }
}