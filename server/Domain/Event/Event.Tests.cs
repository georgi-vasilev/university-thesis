namespace Domain.Event
{
    using Domain.Event.Error;
    using Domain.Event.Factory;
    using FluentAssertions;
    using Xunit;

    public class EventTests
    {
        [Fact]
        public void Can_Create_Valid_Event()
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
                .WithType(VenueType.Indoor)
                .Build();

            var timeRange = new TimeRange(new TimeOnly(10, 0), new TimeOnly(18, 0));

            var organizerId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            var @event = new EventBuilder()
                .WithDate(new DateOnly(2023, 8, 7))
                .WithTime(timeRange)
                .WithVenue(venue.Value)
                .WithOrganizerId(organizerId)
                .WithId(eventId)
                .Build();

            @event.IsError.Should().BeFalse();
            @event.Value.Id.Should().Be(eventId);
        }

        [Fact]
        public void Event_Cannot_Transtion_From_Canclled_To_Active()
        {
            var @event = BuildValueEvent();

            @event.ChangeStatus(EventStatus.Cancelled);
            var result = @event.ChangeStatus(EventStatus.Active);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(EventErrors.InvalidStatusChangeOperationFromCancelledToActive);
        }


        [Fact]
        public void Event_Cannot_Transtion_From_Canclled_To_Postponed()
        {
            var @event = BuildValueEvent();
            @event.ChangeStatus(EventStatus.Cancelled);
            var result = @event.ChangeStatus(EventStatus.Postponed);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(EventErrors.InvalidStatusChangeOperationFromCancelledToPostponed);
        }

        private Event BuildValueEvent()
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
                .WithType(VenueType.Indoor)
                .Build();

            var timeRange = new TimeRange(new TimeOnly(10, 0), new TimeOnly(18, 0));

            var organizerId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            var result = new EventBuilder()
                .WithDate(new DateOnly(2023, 8, 7))
                .WithTime(timeRange)
                .WithVenue(venue.Value)
                .WithOrganizerId(organizerId)
                .WithId(eventId)
                .Build();

            return result.Value;
        }
    }
}
