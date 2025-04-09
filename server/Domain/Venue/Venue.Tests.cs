namespace Domain.Venue
{
    using Builder;
    using Event.Builder;
    using FluentAssertions;
    using Xunit;

    public class VenueTests
    {
        [Fact]
        public void Can_Create_Valid_Venue()
        {
            var address = new AddressBuilder()
                .WithStreet("123 Main St")
                .WithCity("Springfield")
                .Build();

            var venue = new VenueBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Springfield Convention Center")
                .WithAddress(address.Value)
                .WithCapacity(5000)
                .WithType(VenueType.Club)
                .Build();

            venue.IsError.Should().BeFalse();
        }
    }
}
