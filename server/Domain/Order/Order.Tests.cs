namespace Domain.Order
{
    using Builder;
    using Common.ValueObject;
    using Error;
    using ErrorOr;
    using FluentAssertions;
    using Xunit;

    public class OrderTests
    {
        private Order BuildValidOrder()
        {
            var builder = new OrderBuilder().WithBuyer(Guid.NewGuid());
            var orderResult = builder.Build();
            orderResult.IsError.Should().BeFalse();
            return orderResult.Value;
        }

        [Fact]
        public void OrderBuilder_Should_Create_Valid_Order()
        {
            var order = BuildValidOrder();

            order.Should().NotBeNull();
            order.Id.Should().NotBeEmpty();
            order.BuyerId.Should().NotBeEmpty();
            order.Status.Should().Be(OrderStatus.New);
            order.Tickets.Should().BeEmpty();
            order.Payment.Should().BeNull();
        }

        [Fact]
        public void AddTicket_Should_Add_Ticket_To_Order()
        {
            var order = BuildValidOrder();
            var eventId = Guid.NewGuid();
            var money = new Money(100, "USD");
            var ticketType = TicketType.General;

            var result = order.AddTicket(eventId, money, ticketType);

            result.IsError.Should().BeFalse();
            order.Tickets.Should().HaveCount(1);
        }

        [Fact]
        public void RemoveTicket_Should_Remove_Existing_Ticket()
        {
            var order = BuildValidOrder();
            var eventId = Guid.NewGuid();
            var money = new Money(50, "USD");
            var ticketType = TicketType.VIP;
            var addTicketResult = order.AddTicket(eventId, money, ticketType);
            addTicketResult.IsError.Should().BeFalse();
            addTicketResult.Value.Should().Be(Result.Success);
            var ticket = order.Tickets.First();

            var result = order.RemoveTicket(ticket.Id);

            result.IsError.Should().BeFalse();
            order.Tickets.Should().BeEmpty();
        }

        [Fact]
        public void RemoveTicket_Should_Return_Error_When_Ticket_Not_Found()
        {
            var order = BuildValidOrder();
            var nonExistentTicketId = Guid.NewGuid();

            var result = order.RemoveTicket(nonExistentTicketId);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(OrderError.TicketNotFoundError);
        }

        [Fact]
        public void ChangeOrderStatus_Should_Update_Status_When_Valid()
        {
            var order = BuildValidOrder();

            var result = order.ChangeOrderStatus(OrderStatus.Pending);

            result.IsError.Should().BeFalse();
            order.Status.Should().Be(OrderStatus.Pending);
        }

        [Fact]
        public void ChangeOrderStatus_Should_Return_Error_For_Invalid_Transition()
        {
            var order = BuildValidOrder();

            var changeToPendingResult = order.ChangeOrderStatus(OrderStatus.Pending);
            changeToPendingResult.IsError.Should().BeFalse();
            changeToPendingResult.Value.Should().Be(Result.Success);

            var changeOrderToNewResult = order.ChangeOrderStatus(OrderStatus.New);

            changeOrderToNewResult.IsError.Should().BeTrue();
            changeOrderToNewResult.FirstError.Should().Be(OrderError.InvalidOrderStatusChangeOperationError);
        }

        [Fact]
        public void CompleteOrder_Should_Return_Error_When_No_Tickets()
        {
            var order = BuildValidOrder();

            var result = order.CompleteOrder();

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(OrderError.NoTicketsInOrderError);
        }

        [Fact]
        public void CompleteOrder_Should_Set_Status_To_Completed_When_Tickets_Exist()
        {
            var order = BuildValidOrder();
            var eventId = Guid.NewGuid();
            var money = new Money(75, "USD");
            var ticketType = TicketType.General;

            var addTicketResult = order.AddTicket(eventId, money, ticketType);
            addTicketResult.IsError.Should().BeFalse();
            addTicketResult.Value.Should().Be(Result.Success);

            var result = order.CompleteOrder();

            result.IsError.Should().BeFalse();
            order.Status.Should().Be(OrderStatus.Completed);
        }

        [Fact]
        public void MarkTicketAsSold_Should_Mark_Ticket_As_Sold()
        {
            var order = BuildValidOrder();
            var eventId = Guid.NewGuid();
            var money = new Money(80, "USD");
            var ticketType = TicketType.General;
            var addTicketResult = order.AddTicket(eventId, money, ticketType);
            addTicketResult.IsError.Should().BeFalse();
            addTicketResult.Value.Should().Be(Result.Success);
            var ticket = order.Tickets.First();
            var attendeeId = Guid.NewGuid();

            var result = order.MarkTicketAsSold(ticket.Id, attendeeId);

            result.IsError.Should().BeFalse();
            ticket.Status.Should().Be(TicketStatus.Sold);
            ticket.AttendeeId.Should().Be(attendeeId);
        }

        [Fact]
        public void MarkTicketAsSold_Should_Return_Error_For_Nonexistent_Ticket()
        {
            var order = BuildValidOrder();
            var nonExistentTicketId = Guid.NewGuid();
            var attendeeId = Guid.NewGuid();

            var result = order.MarkTicketAsSold(nonExistentTicketId, attendeeId);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(OrderError.TicketNotFoundError);
        }

        [Fact]
        public void UpdatePaymentDetails_Should_Update_Payment_Property()
        {
            var order = BuildValidOrder();
            var paymentDetails = new PaymentDetails(150, "Stripe", PaymentStatus.Pending, "txn_123");

            var result = order.UpdatePaymentDetails(paymentDetails);

            result.IsError.Should().BeFalse();
            order.Payment.Should().Be(paymentDetails);
        }

        [Fact]
        public void MarkPaymentAsCompleted_Should_Update_Payment_And_Status()
        {
            var order = BuildValidOrder();
            var completedPayment = new PaymentDetails(200, "Stripe", PaymentStatus.Completed, "txn_456");

            var result = order.MarkPaymentAsCompleted(completedPayment);

            result.IsError.Should().BeFalse();
            order.Payment.Should().Be(completedPayment);
            order.Status.Should().Be(OrderStatus.Completed);
        }
    }
}
