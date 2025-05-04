namespace Domain.Order
{
    using Builder;
    using Common.Error;
    using Common.ValueObject;
    using Error;
    using ErrorOr;
    using FluentAssertions;
    using System;
    using Xunit;

    public class TicketTests
    {
        [Fact]
        public void TicketBuilder_Should_Create_Valid_Ticket()
        {
            var eventId = Guid.NewGuid();
            var price = new Money(50, "USD");
            var ticketType = TicketType.General;

            var result = new TicketBuilder()
                .WithEventId(eventId)
                .WithPrice(price)
                .WithType(ticketType)
                .Build();

            result.IsError.Should().BeFalse();
            var ticket = result.Value;
            ticket.EventId.Should().Be(eventId);
            ticket.Price.Should().Be(price);
            ticket.Type.Should().Be(ticketType);
            ticket.Status.Should().Be(TicketStatus.Available);
            ticket.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void TicketBuilder_Should_Return_Error_When_EventId_Is_Empty()
        {
            var price = new Money(50, "USD");
            var ticketType = TicketType.VIP;

            var result = new TicketBuilder()
                .WithEventId(Guid.Empty)
                .WithPrice(price)
                .WithType(ticketType)
                .Build();

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(TicketError.InvalidEventError);
        }

        [Fact]
        public void TicketBuilder_Should_Return_Error_When_Price_Is_Null()
        {
            var eventId = Guid.NewGuid();
            var ticketType = TicketType.General;
            Money? price = null;

            var result = new TicketBuilder()
                .WithEventId(eventId)
                .WithPrice(price)
                .WithType(ticketType)
                .Build();

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(MoneyError.NullError);
        }

        [Fact]
        public void TicketBuilder_Should_Return_Error_When_Price_Amount_Is_NonPositive()
        {
            var eventId = Guid.NewGuid();
            var price = new Money(0, "USD");
            var ticketType = TicketType.General;

            var result = new TicketBuilder()
                .WithEventId(eventId)
                .WithPrice(price)
                .WithType(ticketType)
                .Build();

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(MoneyError.InvalidAmountError);
        }

        [Fact]
        public void AddToBuyer_Should_Update_Ticket_Status_And_Set_AttendeeId()
        {
            var eventId = Guid.NewGuid();
            var price = new Money(100, "USD");
            var ticketType = TicketType.General;
            var buildResult = new TicketBuilder()
                .WithEventId(eventId)
                .WithPrice(price)
                .WithType(ticketType)
                .Build();
            buildResult.IsError.Should().BeFalse();
            var ticket = buildResult.Value;
            var attendeeId = Guid.NewGuid();

            var result = ticket.AddToBuyer(attendeeId);

            result.IsError.Should().BeFalse();
            ticket.Status.Should().Be(TicketStatus.Sold);
            ticket.AttendeeId.Should().Be(attendeeId);
        }

        [Fact]
        public void AddToBuyer_Should_Return_Error_If_Ticket_Already_Sold()
        {
            var eventId = Guid.NewGuid();
            var price = new Money(75, "USD");
            var ticketType = TicketType.General;
            var buildResult = new TicketBuilder()
                .WithEventId(eventId)
                .WithPrice(price)
                .WithType(ticketType)
                .Build();
            buildResult.IsError.Should().BeFalse();
            var ticket = buildResult.Value;
            var firstAttendeeId = Guid.NewGuid();
            var AddToBuyerResult = ticket.AddToBuyer(firstAttendeeId);
            AddToBuyerResult.IsError.Should().BeFalse();
            AddToBuyerResult.Value.Should().Be(Result.Success);

            var secondAttendeeId = Guid.NewGuid();

            var result = ticket.AddToBuyer(secondAttendeeId);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(TicketError.AlreadySoldError);
        }
    }
}

