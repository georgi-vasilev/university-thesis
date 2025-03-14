namespace Domain.Event
{
    using Domain.Event.Error;
    using Domain.Event.Factory;
    using FluentAssertions;
    using System;
    using Xunit;

    public class AdditionalEventTests
    {
        [Fact]
        public void Reschedule_Should_Fail_When_New_Date_Is_In_The_Past()
        {
            var @event = BuildValueEvent();
            var pastStart = DateTime.Now.AddDays(-1).Date + new TimeSpan(10, 0, 0);
            var pastEnd = pastStart.AddHours(2);

            var result = @event.Reschedule(pastStart, pastEnd);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(EventErrors.DateIsInThePastError);
        }

        [Fact]
        public void Reschedule_Should_Succeed_When_New_Date_Is_In_Future_And_No_Tickets_Sold()
        {
            var @event = BuildValueEvent();
            var futureStart = DateTime.Now.AddDays(2).Date + new TimeSpan(12, 0, 0);
            var futureEnd = futureStart.AddHours(2);

            var result = @event.Reschedule(futureStart, futureEnd);

            result.IsError.Should().BeFalse();
            @event.Date.Should().Be(DateOnly.FromDateTime(futureStart));

            @event.Time.Should().Be(TimeRange.FromDateTimes(futureStart, futureEnd).Value);
        }

        [Fact]
        public void UpdateDetails_Should_Fail_When_Name_Is_Empty()
        {
            var @event = BuildValueEvent();

            var result = @event.UpdateDetails("", "Updated Description", new DateOnly(2023, 9, 1), @event.Time, @event.VenueId);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(EventErrors.InvalidName);
        }

        [Fact]
        public void UpdateDetails_Should_Fail_When_Description_Is_Empty()
        {
            var @event = BuildValueEvent();

            var result = @event.UpdateDetails("Updated Name", "", new DateOnly(2023, 9, 1), @event.Time, @event.VenueId);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(EventErrors.InvalidDescription);
        }

        [Fact]
        public void UpdateDetails_Should_Succeed_With_Valid_Data()
        {
            var @event = BuildValueEvent();
            var newName = "Updated Event Name";
            var newDescription = "Updated Description";
            var newDate = new DateOnly(2026, 9, 1);
            var newTime = new TimeRange(new DateTime(11, 0), new DateTime(19, 0));
            var newVenueId = Guid.NewGuid();

            var result = @event.UpdateDetails(newName, newDescription, newDate, newTime, newVenueId);

            result.IsError.Should().BeFalse();
            @event.Name.Should().Be(newName);
            @event.Description.Should().Be(newDescription);
            @event.Date.Should().Be(newDate);
            @event.Time.Should().Be(newTime);
            @event.VenueId.Should().Be(newVenueId);
        }

        [Fact]
        public void AddTicket_Should_Succeed_If_Capacity_Not_Exceeded()
        {
            var @event = BuildValueEvent();
            var initialTicketCount = @event.TicketCount;

            var ticketId = Guid.NewGuid();
            var result = @event.AddTicket(ticketId);

            result.IsError.Should().BeFalse();
            @event.TicketCount.Should().Be(initialTicketCount + 1);
        }

        [Fact]
        public void AddTicket_Should_Fail_If_Capacity_Exceeded()
        {
            var smallCapacityEvent = new EventBuilder()
                .WithName("DNB")
                .WithDescription("DnB at mixtape")
                .WithDate(new DateOnly(2026, 8, 7))
                .WithTime(new TimeRange(new DateTime(10, 0), new DateTime(18, 0)))
                .WithCapacity(1)
                .WithVenue(BuildValueEvent().VenueId)
                .WithHostId(Guid.NewGuid())
                .Build().Value;

            var firstResult = smallCapacityEvent.AddTicket(Guid.NewGuid());
            firstResult.IsError.Should().BeFalse();

            var secondResult = smallCapacityEvent.AddTicket(Guid.NewGuid());

            secondResult.IsError.Should().BeTrue();
            secondResult.FirstError.Should().Be(EventErrors.CapacityExceeded);
        }

        [Fact]
        public void TimeRange_Should_Overlap_With_Overlapping_Range()
        {
            var timeRange1 = new TimeRange(new DateTime(10, 0), new DateTime(14, 0));
            var timeRange2 = new TimeRange(new DateTime(13, 0), new DateTime(16, 0));

            timeRange1.OverlapsWith(timeRange2).Should().BeTrue();
        }

        [Fact]
        public void TimeRange_Should_Not_Overlap_With_NonOverlapping_Range()
        {
            var timeRange1 = new TimeRange(new DateTime(10, 0), new DateTime(12, 0));
            var timeRange2 = new TimeRange(new DateTime(13, 0), new DateTime(16, 0));

            timeRange1.OverlapsWith(timeRange2).Should().BeFalse();
        }

        private Event BuildValueEvent()
        {
            var addressResult = new AddressBuilder()
                .WithStreet("123 Main St")
                .WithCity("Springfield")
                .Build();

            addressResult.IsError.Should().BeFalse();
            var address = addressResult.Value;

            var venueResult = new VenueBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Springfield Convention Center")
                .WithAddress(address)
                .WithCapacity(5000)
                .WithType(VenueType.Club)
                .Build();

            venueResult.IsError.Should().BeFalse();
            var venue = venueResult.Value;

            var timeRange = new TimeRange(new DateTime(10, 0), new DateTime(18, 0));

            var organizerId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            var result = new EventBuilder()
                .WithName("DNB")
                .WithDescription("DnB at mixtape")
                .WithDate(new DateOnly(2026, 8, 7))
                .WithTime(timeRange)
                .WithVenue(venue.Id)
                .WithCapacity(1)
                .WithHostId(organizerId)
                .WithId(eventId)
                .Build();

            return result.Value;
        }
    }
}
