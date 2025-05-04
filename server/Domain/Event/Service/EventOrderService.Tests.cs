//namespace Domain.Event.Service
//{
//    using Common.ValueObject;
//    using Domain.Venue.Repository;
//    using Domain.Venue;
//    using Error;
//    using ErrorOr;
//    using FluentAssertions;
//    using Host;
//    using Moq;
//    using Order;
//    using Order.Builder;
//    using Order.Error;
//    using Order.Repository;
//    using Repository;
//    using Xunit;
//    using Domain.Event.Builder;
//    using Domain.Venue.Builder;

//    public class EventOrderServiceTests
//    {
//        private readonly Mock<IEventDomainRepository> _mockEventRepository;
//        private readonly Mock<IOrderDomainRepository> _mockOrderRepository;
//        private readonly Mock<IOrderBuilder> _mockOrderBuilder;
//        private readonly Mock<ITicketBuilder> _mockTicketBuilder;
//        private readonly Mock<IVenueDomainRepository> _mockVenueRepository;
//        private readonly IEventOrderService _service;

//        public EventOrderServiceTests()
//        {
//            _mockEventRepository = new Mock<IEventDomainRepository>();
//            _mockOrderRepository = new Mock<IOrderDomainRepository>();
//            _mockOrderBuilder = new Mock<IOrderBuilder>();
//            _mockTicketBuilder = new Mock<ITicketBuilder>();
//            _mockVenueRepository = new Mock<IVenueDomainRepository>();

//            _service = new EventOrderService(
//                _mockEventRepository.Object,
//                _mockOrderRepository.Object,
//                _mockOrderBuilder.Object,
//                _mockTicketBuilder.Object,
//                _mockVenueRepository.Object);
//        }

//        [Fact]
//        public async Task CancelTicketOrderAsync_Should_Return_OrderNotFoundError_When_No_Order_Matches()
//        {
//            var buyerId = Guid.NewGuid();
//            var ticketId = Guid.NewGuid();
//            _mockOrderRepository
//                .Setup(r => r.GetOrderAsync(It.IsAny<Func<Order, bool>>()))
//                .ReturnsAsync((Order)null);

//            var result = await _service.CancelTicketOrderAsync(buyerId, ticketId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(OrderError.OrderNotFoundError);
//        }

//        [Fact]
//        public async Task CancelTicketOrderAsync_Should_Return_OrderAlreadyCompletedError_When_Order_Is_Completed()
//        {
//            var buyerId = Guid.NewGuid();
//            var ticketId = Guid.NewGuid();
//            var order = CreateValidOrder(buyerId);
//            var changeOrderResult = order.ChangeOrderStatus(OrderStatus.Completed);
//            changeOrderResult.IsError.Should().BeFalse();
//            changeOrderResult.Value.Should().Be(Result.Success);

//            var addTicketResult = order.AddTicket(new Ticket(Guid.NewGuid(), new Money(50, "USD"), TicketType.General));
//            addTicketResult.IsError.Should().BeFalse();
//            addTicketResult.Value.Should().Be(Result.Success);

//            _mockOrderRepository
//                .Setup(r => r.GetOrderAsync(It.IsAny<Func<Order, bool>>()))
//                .ReturnsAsync(order);

//            var result = await _service.CancelTicketOrderAsync(buyerId, ticketId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(OrderError.OrderAlreadyCompletedError);
//        }

//        [Fact]
//        public async Task CancelTicketOrderAsync_Should_Return_TicketNotFoundError_When_Ticket_Not_In_Order()
//        {
//            var buyerId = Guid.NewGuid();
//            var ticketId = Guid.NewGuid();
//            var order = CreateValidOrder(buyerId);

//            _mockOrderRepository
//                .Setup(r => r.GetOrderAsync(It.IsAny<Func<Order, bool>>()))
//                .ReturnsAsync(order);

//            var result = await _service.CancelTicketOrderAsync(buyerId, ticketId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(OrderError.TicketNotFoundError);
//        }

//        [Fact]
//        public async Task CancelTicketOrderAsync_Should_Return_TicketAlreadyUsedError_When_Ticket_HasBeenUsed()
//        {
//            var buyerId = Guid.NewGuid();
//            var ticketId = Guid.NewGuid();
//            var order = CreateValidOrder(buyerId);

//            var ticket = CreateValidTicket(ticketId);

//            var markAsSoldResult = ticket.MarkAsSold(Guid.NewGuid());
//            markAsSoldResult.IsError.Should().BeFalse();
//            markAsSoldResult.Value.Should().Be(Result.Success);

//            ticket.GetType().GetProperty("HasBeenUsed")?.SetValue(ticket, true);
//            var addTicketResult = order.AddTicket(ticket);
//            addTicketResult.IsError.Should().BeFalse();
//            addTicketResult.Value.Should().Be(Result.Success);

//            _mockOrderRepository
//                .Setup(r => r.GetOrderAsync(It.IsAny<Func<Order, bool>>()))
//                .ReturnsAsync(order);

//            var result = await _service.CancelTicketOrderAsync(buyerId, ticketId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(OrderError.TicketAlreadyUsedError);
//        }

//        [Fact]
//        public async Task CancelTicketOrderAsync_Should_Succeed_When_Order_Remains_With_Tickets()
//        {
//            var buyerId = Guid.NewGuid();
//            var ticketId = Guid.NewGuid();
//            var order = CreateValidOrder(buyerId);
//            var ticket = CreateValidTicket(ticketId);
//            var addTicketResult = order.AddTicket(ticket);
//            addTicketResult.IsError.Should().BeFalse();
//            addTicketResult.Value.Should().Be(Result.Success);

//            var changeOrderStatusResult = order.ChangeOrderStatus(OrderStatus.New);
//            changeOrderStatusResult.Value.Should().Be(Result.Success);

//            _mockOrderRepository
//                .Setup(r => r.GetOrderAsync(It.IsAny<Func<Order, bool>>()))
//                .ReturnsAsync(order);
//            _mockOrderRepository
//                .Setup(r => r.UpdateAsync(order, new CancellationToken()))
//                .Returns(Task.CompletedTask);

//            var result = await _service.CancelTicketOrderAsync(buyerId, ticketId, new CancellationToken());

//            result.IsError.Should().BeFalse();
//            order.Tickets.Should().BeEmpty();
//        }

//        [Fact]
//        public async Task CancelTicketOrderAsync_Should_Succeed_And_Mark_OrderAsCancelled_When_No_Tickets_Remain()
//        {
//            var buyerId = Guid.NewGuid();
//            var ticketId = Guid.NewGuid();
//            var order = CreateValidOrder(buyerId);
//            var ticket = CreateValidTicket(ticketId);
//            var addTicketResult = order.AddTicket(ticket);
//            addTicketResult.IsError.Should().BeFalse();
//            addTicketResult.Value.Should().Be(Result.Success);

//            var changeOrderStatusResult = order.ChangeOrderStatus(OrderStatus.New);
//            changeOrderStatusResult.Value.Should().Be(Result.Success);
//            _mockOrderRepository
//                .Setup(r => r.GetOrderAsync(It.IsAny<Func<Order, bool>>()))
//                .ReturnsAsync(order);
//            _mockOrderRepository
//                .Setup(r => r.UpdateAsync(order, new CancellationToken()))
//                .Returns(Task.CompletedTask);

//            var result = await _service.CancelTicketOrderAsync(buyerId, ticketId, new CancellationToken());

//            result.IsError.Should().BeFalse();
//            order.Tickets.Should().BeEmpty();
//            order.Status.Should().Be(OrderStatus.Cancelled);
//        }

//        [Fact]
//        public async Task ChangeEventVenueAsync_Should_Return_EventNotFoundError_When_Event_Not_Found()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                                .ReturnsAsync((Event)null);

//            var result = await _service.ChangeEventVenueAsync(host, eventId, Guid.NewGuid(), new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventNotFoundError);
//        }

//        [Fact]
//        public async Task ChangeEventVenueAsync_Should_Return_EventDoesNotBelongToHostError_When_HostMismatch()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();

//            var venueId = Guid.NewGuid();
//            var @event = CreateValidEvent(Guid.NewGuid(), eventId, venueId);
//            _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                                .ReturnsAsync(@event);

//            var result = await _service.ChangeEventVenueAsync(host, eventId, Guid.NewGuid(), new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventDoesNotBelongToHostError);
//        }

//        [Fact]
//        public async Task ChangeEventVenueAsync_Should_Return_Error_If_UpdateDetails_Fails()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var @event = CreateValidEvent(host.Id, eventId, venueId);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            var result = await _service.ChangeEventVenueAsync(host, eventId, Guid.Empty, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.NullVenueError);
//        }

//        [Fact]
//        public async Task ChangeEventVenueAsync_Should_Succeed_When_AllValid()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var newVenueId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var @event = CreateValidEvent(host.Id, eventId, venueId);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            var result = await _service.ChangeEventVenueAsync(host, eventId, newVenueId, new CancellationToken());

//            result.IsError.Should().BeFalse();
//            @event.VenueId.Should().Be(newVenueId);
//        }

//        [Fact]
//        public async Task CompleteOrderAsync_Should_Return_OrderNotFoundError_When_Order_Not_Found()
//        {
//            var orderId = Guid.NewGuid();
//            _mockOrderRepository
//                .Setup(r => r.GetByIdAsync(orderId, new CancellationToken()))
//                 .ReturnsAsync((Order)null);

//            var result = await _service.CompleteOrderAsync(orderId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(OrderError.OrderNotFoundError);
//        }

//        [Fact]
//        public async Task CompleteOrderAsync_Should_Return_Error_If_CompleteOrder_Fails()
//        {
//            var order = CreateValidOrder(Guid.NewGuid());

//            _mockOrderRepository
//                .Setup(r => r.GetByIdAsync(order.Id, new CancellationToken()))
//                .ReturnsAsync(order);

//            var result = await _service.CompleteOrderAsync(order.Id, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(OrderError.NoTicketsInOrderError);
//        }

//        [Fact]
//        public async Task CompleteOrderAsync_Should_Succeed_When_AllValid()
//        {
//            var order = CreateValidOrder(Guid.NewGuid());

//            var addTicketResult = order.AddTicket(CreateValidTicket(Guid.NewGuid()));
//            addTicketResult.IsError.Should().BeFalse();
//            addTicketResult.Value.Should().Be(Result.Success);

//            _mockOrderRepository
//                .Setup(r => r.GetByIdAsync(order.Id, new CancellationToken()))
//                .ReturnsAsync(order);

//            _mockOrderRepository
//                .Setup(r => r.UpdateAsync(order, new CancellationToken()))
//                .Returns(Task.CompletedTask);

//            var result = await _service.CompleteOrderAsync(order.Id, new CancellationToken());

//            result.IsError.Should().BeFalse();
//            order.Status.Should().Be(OrderStatus.Completed);
//            _mockOrderRepository.Verify(r => r.UpdateAsync(order, new CancellationToken()), Times.Once);
//        }

//        [Fact]
//        public async Task PurchaseTicketAsync_Should_Return_EventNotFoundError_When_Event_Is_Null()
//        {
//            var buyerId = Guid.NewGuid();
//            var eventId = Guid.NewGuid();
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync((Event)null);

//            var result = await _service.PurchaseTicketAsync(buyerId, eventId, new Money(100, "USD"), TicketType.General, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventNotFoundError);
//        }

//        [Fact]
//        public async Task PurchaseTicketAsync_Should_Return_NoTicketsLeftError_When_Capacity_Reached()
//        {
//            var buyerId = Guid.NewGuid();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var @event = CreateValidEvent(Guid.NewGuid(), eventId, venueId);
//            var addressResult = new AddressBuilder()
//               .WithStreet("123 Main St")
//               .WithCity("Springfield")
//               .Build();
//            var venue = new VenueBuilder()
//                .WithId(venueId)
//                .WithCapacity(20)
//                .WithType(VenueType.Club)
//                .WithAddress(addressResult.Value)
//                .WithName("Mixtape")
//                .Build()
//                .Value;
//            _mockVenueRepository
//               .Setup(x => x.GetByIdAsync(venueId, new CancellationToken()))
//               .ReturnsAsync(venue);

//            for (int i = 0; i < venue.Capacity; i++)
//            {
//                @event.AddTicket(Guid.NewGuid(), venue.Capacity);
//            }

//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            var result = await _service.PurchaseTicketAsync(buyerId, eventId, new Money(100, "USD"), TicketType.General, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.NoTicketsLeftError);
//        }

//        [Fact]
//        public async Task PurchaseTicketAsync_Should_Return_EventHasEndedOrCancelledError_When_Event_Not_Active()
//        {
//            var buyerId = Guid.NewGuid();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var @event = CreateValidEvent(Guid.NewGuid(), eventId, venueId);
//            MockVenue(venueId);

//            @event.ChangeStatus(EventStatus.Cancelled);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            var result = await _service.PurchaseTicketAsync(buyerId, eventId, new Money(100, "USD"), TicketType.General, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventHasEndedOrCancelledError);
//        }

//        [Fact]
//        public async Task PurchaseTicketAsync_Should_Return_Error_If_OrderBuilder_Fails()
//        {
//            var buyerId = Guid.NewGuid();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var @event = CreateValidEvent(Guid.NewGuid(), eventId, venueId);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);
//            _mockOrderBuilder
//                .Setup(b => b.WithBuyer(buyerId))
//                .Returns(_mockOrderBuilder.Object);

//            var orderError = OrderError.InvalidBuyerError;
//            _mockOrderBuilder
//                .Setup(b => b.Build())
//                .Returns((ErrorOr<Order>)orderError);
//            MockVenue(venueId);

//            var result = await _service.PurchaseTicketAsync(buyerId, eventId, new Money(100, "USD"), TicketType.General, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(orderError);
//        }

//        [Fact]
//        public async Task PurchaseTicketAsync_Should_Return_Error_If_TicketBuilder_Fails()
//        {
//            var buyerId = Guid.NewGuid();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var @event = CreateValidEvent(Guid.NewGuid(), eventId, venueId);
//            _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                                .ReturnsAsync(@event);
//            var order = CreateValidOrder(buyerId);
//            _mockOrderBuilder
//                .Setup(b => b.WithBuyer(buyerId))
//                .Returns(_mockOrderBuilder.Object);
//            _mockOrderBuilder
//                .Setup(b => b.Build())
//                .Returns((ErrorOr<Order>)order);
//            var ticketError = TicketError.InvalidEventError;
//            _mockTicketBuilder
//                .Setup(b => b.WithEventId(eventId))
//                .Returns(_mockTicketBuilder.Object);
//            _mockTicketBuilder
//                .Setup(b => b.WithPrice(It.IsAny<Money>()))
//                .Returns(_mockTicketBuilder.Object);
//            _mockTicketBuilder
//                .Setup(b => b.WithType(It.IsAny<TicketType>()))
//                .Returns(_mockTicketBuilder.Object);
//            _mockTicketBuilder
//                .Setup(b => b.Build())
//                .Returns((ErrorOr<Ticket>)ticketError);
//            MockVenue(venueId);


//            var result = await _service.PurchaseTicketAsync(buyerId, eventId, new Money(100, "USD"), TicketType.General, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(ticketError);
//        }

//        [Fact]
//        public async Task PurchaseTicketAsync_Should_Return_Error_If_Order_AddTicket_Fails()
//        {
//            var buyerId = Guid.NewGuid();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var @event = CreateValidEvent(Guid.NewGuid(), eventId, venueId);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            var order = CreateValidOrder(buyerId);
//            _mockOrderBuilder
//                .Setup(b => b.WithBuyer(buyerId))
//                .Returns(_mockOrderBuilder.Object);
//            _mockOrderBuilder
//                .Setup(b => b.Build())
//                .Returns((ErrorOr<Order>)order);

//            var ticket = CreateValidTicket(Guid.NewGuid());
//            _mockTicketBuilder
//                .Setup(b => b.WithEventId(eventId))
//                .Returns(_mockTicketBuilder.Object);
//            _mockTicketBuilder
//                .Setup(b => b.WithPrice(It.IsAny<Money>()))
//                .Returns(_mockTicketBuilder.Object);
//            _mockTicketBuilder
//                .Setup(b => b.WithType(It.IsAny<TicketType>()))
//                .Returns(_mockTicketBuilder.Object);
//            _mockTicketBuilder
//                .Setup(b => b.Build())
//                .Returns((ErrorOr<Ticket>)ticket);

//            MockVenue(venueId);

//             var preAddResult = order.AddTicket(ticket);
//            preAddResult.IsError.Should().BeFalse();

//            var result = await _service.PurchaseTicketAsync(buyerId, eventId, new Money(100, "USD"), TicketType.General, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(OrderError.TicketAlreadyAddedError);
//        }


//        [Fact]
//        public async Task PurchaseTicketAsync_Should_Succeed_When_AllValid()
//        {
//            var buyerId = Guid.NewGuid();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var price = new Money(100, "USD");
//            var @event = CreateValidEvent(Guid.NewGuid(), eventId, venueId);

//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);
//            _mockOrderRepository
//                .Setup(r => r.UpdateAsync(It.IsAny<Order>(), new CancellationToken()))
//                .Returns(Task.CompletedTask);

//            var order = CreateValidOrder(buyerId);
//            _mockOrderBuilder
//                .Setup(b => b.WithBuyer(buyerId))
//                .Returns(_mockOrderBuilder.Object);
//            _mockOrderBuilder
//                .Setup(b => b.Build())
//                .Returns((ErrorOr<Order>)order);

//            var ticket = CreateValidTicket(Guid.NewGuid());
//            _mockTicketBuilder
//                .Setup(b => b.WithEventId(eventId))
//                .Returns(_mockTicketBuilder.Object);
//            _mockTicketBuilder
//                .Setup(b => b.WithPrice(price))
//                .Returns(_mockTicketBuilder.Object);
//            _mockTicketBuilder
//                .Setup(b => b.WithType(TicketType.General))
//                .Returns(_mockTicketBuilder.Object);
//            _mockTicketBuilder
//                .Setup(b => b.Build())
//                .Returns((ErrorOr<Ticket>)ticket);
//            MockVenue(venueId);

//            var result = await _service.PurchaseTicketAsync(buyerId, eventId, price, TicketType.General, new CancellationToken());

//            result.IsError.Should().BeFalse();
//            _mockOrderRepository.Verify(r => r.UpdateAsync(order, new CancellationToken()), Times.Once);
//        }


//        [Fact]
//        public async Task UpdateEventDetailsAsync_Should_Return_Error_When_HostMismatch()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var updatedEvent = CreateValidEvent(Guid.NewGuid(), eventId, venueId);

//            var result = await _service.UpdateEventDetailsAsync(host, updatedEvent, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventDoesNotBelongToHostError);
//        }

//        [Fact]
//        public async Task UpdateEventDetailsAsync_Should_Return_Error_When_Event_Not_Found()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var updatedEvent = CreateValidEvent(host.Id, eventId, venueId);
//            _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                                .ReturnsAsync((Event)null);

//            var result = await _service.UpdateEventDetailsAsync(host, updatedEvent, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventNotFoundError);
//        }

//        [Fact]
//        public async Task UpdateEventDetailsAsync_Should_Return_Error_If_UpdateDetails_Fails()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var existingEvent = CreateValidEvent(host.Id, eventId, venueId);
//            var updatedEvent = CreateValidEvent(host.Id, eventId, venueId);
//            updatedEvent = new Event(updatedEvent.Name, updatedEvent.Description, updatedEvent.Date, updatedEvent.Time, Guid.Empty, host.Id, updatedEvent.Id);

//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(existingEvent);

//            var result = await _service.UpdateEventDetailsAsync(host, updatedEvent, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.NullVenueError);
//        }

//        [Fact]
//        public async Task UpdateEventDetailsAsync_Should_Succeed_When_AllValid()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var venueId = Guid.NewGuid();
//            var existingEvent = CreateValidEvent(host.Id, eventId, venueId);
//            var updatedEvent = CreateValidEvent(host.Id, eventId, venueId);

//            updatedEvent = new Event("Updated Concert", "Updated Description", existingEvent.Date, existingEvent.Time, Guid.NewGuid(), host.Id, existingEvent.Id);

//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(existingEvent);
//            _mockEventRepository
//                .Setup(r => r.UpdateAsync(existingEvent, new CancellationToken()))
//                .Returns(Task.CompletedTask);

//            var result = await _service.UpdateEventDetailsAsync(host, updatedEvent, new CancellationToken());

//            result.IsError.Should().BeFalse();
//            existingEvent.Name.Should().Be("Updated Concert");
//            existingEvent.Description.Should().Be("Updated Description");
//            _mockEventRepository.Verify(r => r.UpdateAsync(existingEvent, new CancellationToken()), Times.Once);
//        }

//        private Host CreateValidHost()
//        {
//            var contactInfo = new ContactInfo(
//                Guid.NewGuid(), "John", "Doe",
//                "111-222-3333", "john.doe@example.com", "john_doe");
//            return new Host(contactInfo, Guid.NewGuid());
//        }

//        private Event CreateValidEvent(Guid hostId, Guid eventId, Guid venueId)
//        {
//            var date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));
//            var timeRangeResult = TimeRange.FromDateTimes(DateTime.UtcNow.AddDays(10).AddHours(12),
//                                                           DateTime.UtcNow.AddDays(10).AddHours(14));
//            timeRangeResult.IsError.Should().BeFalse();
//            return new Event(
//                "Concert",
//                "A great concert",
//                date,
//                timeRangeResult.Value,
//                venueId,
//                hostId,
//                eventId);
//        }

//        private Order CreateValidOrder(Guid buyerId)
//        {
//            return new Order(buyerId);
//        }

//        private Ticket CreateValidTicket(Guid ticketId)
//        {
//            return new Ticket(Guid.NewGuid(), new Money(50, "USD"), TicketType.General, ticketId);
//        }

//        private void MockVenue(Guid venueId)
//        {
//            var venue = new VenueBuilder()
//                .WithId(venueId)
//                .WithCapacity(1)
//                .WithName("Mixtape")
//                .WithType(VenueType.Club)
//                .WithAddress(
//                    new AddressBuilder()
//                        .WithCity("Sofia")
//                        .WithStreet("some street")
//                        .Build().Value
//                )
//                .Build().Value;

//            _mockVenueRepository
//                .Setup(x => x.GetByIdAsync(venueId, new CancellationToken()))
//                .ReturnsAsync(venue);
//        }
//    }
//}