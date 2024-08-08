namespace Domain.Event
{
    using Domain.Event.Factory;
    using FluentAssertions;
    using Xunit;

    public class AddressTests
    {
        [Fact]
        public void Can_Create_Valid_Address()
        {
            var address = new AddressBuilder()
               .WithStreet("123 Main St")
               .WithCity("Springfield")
               .Build();

            address.Should().NotBeNull();
            address.IsError.Should().BeFalse();
        }
    }
}
