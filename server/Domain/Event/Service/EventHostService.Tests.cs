//namespace Domain.Event.Service
//{
//    using Error;
//    using ErrorOr;
//    using FluentAssertions;
//    using Host;
//    using Host.Repository;
//    using Moq;
//    using Order;
//    using Order.Builder;
//    using Order.Repository;
//    using Repository;
//    using System;
//    using System.Collections.Generic;
//    using System.Threading.Tasks;
//    using Xunit;

//    public class EventHostServiceTests
//    {
//        private readonly Mock<IEventDomainRepository> _mockEventRepository;
//        private readonly Mock<IHostDomainRepository> _mockHostRepository;
//        private readonly Mock<IOrderDomainRepository> _mockOrderRepository;
//        private readonly Mock<ITicketBuilder> _mockTicketBuilder;
//        private readonly IEventHostService _service;

//        public EventHostServiceTests()
//        {
//            _mockEventRepository = new Mock<IEventDomainRepository>();
//            _mockHostRepository = new Mock<IHostDomainRepository>();
//            _mockOrderRepository = new Mock<IOrderDomainRepository>();
//            _mockTicketBuilder = new Mock<ITicketBuilder>();
//            _service = new EventHostService(_mockEventRepository.Object, _mockHostRepository.Object, _mockOrderRepository.Object);
//        }


//        [Fact]
//        public async Task CancelEventForHostAsync_Should_Return_EventNotFoundError_When_EventIsNull()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync((Event)null);

//            var result = await _service.CancelEventForHostAsync(host, eventId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventNotFoundError);
//        }

//        [Fact]
//        public async Task CancelEventForHostAsync_Should_Return_EventDoesNotBelongToHostError_When_HostMismatch()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();

//            var @event = CreateValidEvent(hostId: Guid.NewGuid(), eventId: eventId);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            var result = await _service.CancelEventForHostAsync(host, eventId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventDoesNotBelongToHostError);
//        }

//        [Fact]
//        public async Task CancelEventForHostAsync_Should_Return_Success_From_ChangeStatus_When_Change_To_Cancelled()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();

//            var @event = CreateValidEvent(host.Id, eventId);

//            @event.ChangeStatus(EventStatus.Cancelled);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            var result = await _service.CancelEventForHostAsync(host, eventId, new CancellationToken());

//            result.IsError.Should().BeFalse();

//            result.Value.Should().Be(Result.Success);
//        }

//        [Fact]
//        public async Task CancelEventForHostAsync_Should_Succeed_When_AllConditionsAreMet()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var @event = CreateValidEvent(host.Id, eventId);
//            _mockEventRepository.Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                                .ReturnsAsync(@event);
//            _mockEventRepository.Setup(r => r.UpdateAsync(@event, new CancellationToken()))
//                                .Returns(Task.CompletedTask);

//            var result = await _service.CancelEventForHostAsync(host, eventId, new CancellationToken());

//            result.IsError.Should().BeFalse();
//            _mockEventRepository.Verify(r => r.UpdateAsync(@event, new CancellationToken()), Times.Once);
//        }



//        [Fact]
//        public async Task CreateEventForHostAsync_Should_Return_Error_When_HostMismatch()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();

//            var @event = CreateValidEvent(Guid.NewGuid(), eventId);

//            var result = await _service.CreateEventForHostAsync(host, @event, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventDoesNotBelongToHostError);
//        }

//        [Fact]
//        public async Task CreateEventForHostAsync_Should_Succeed_When_AllValid()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var @event = CreateValidEvent(host.Id, eventId);
//            _mockEventRepository
//                .Setup(r => r.AddAsync(@event, new CancellationToken()))
//                .Returns(Task.CompletedTask);
//            _mockHostRepository
//                .Setup(r => r.UpdateAsync(host, new CancellationToken()))
//                .Returns(Task.CompletedTask);

//            var result = await _service.CreateEventForHostAsync(host, @event, new CancellationToken());

//            result.IsError.Should().BeFalse();
//            _mockEventRepository.Verify(r => r.AddAsync(@event, new CancellationToken()), Times.Once);
//            _mockHostRepository.Verify(r => r.UpdateAsync(host, new CancellationToken()), Times.Once);
//        }


//        [Fact]
//        public async Task RemoveEventFromHostAsync_Should_Return_Error_When_EventNotFound()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync((Event)null);

//            var result = await _service.RemoveEventFromHostAsync(host, eventId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventNotFoundError);
//        }

//        [Fact]
//        public async Task RemoveEventFromHostAsync_Should_Return_Error_When_HostMismatch()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();

//            var @event = CreateValidEvent(Guid.NewGuid(), eventId);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            var result = await _service.RemoveEventFromHostAsync(host, eventId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.EventDoesNotBelongToHostError);
//        }

//        [Fact]
//        public async Task RemoveEventFromHostAsync_Should_Return_Error_When_OrderCompletedExists()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var @event = CreateValidEvent(host.Id, eventId);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            host.AddOrganizedEvent(eventId);

//            var completedOrder = new Order(Guid.NewGuid());
//            completedOrder.ChangeOrderStatus(OrderStatus.Completed);
//            _mockOrderRepository
//                .Setup(r => r.GetOrdersForEventAsync(eventId))
//                .ReturnsAsync(new List<Order> { completedOrder });

//            var result = await _service.RemoveEventFromHostAsync(host, eventId, new CancellationToken());

//            result.IsError.Should().BeTrue();
//            result.FirstError.Should().Be(EventErrors.CannotDeleteEventWithOrders);
//        }

//        [Fact]
//        public async Task RemoveEventFromHostAsync_Should_Succeed_When_AllValid()
//        {
//            var host = CreateValidHost();
//            var eventId = Guid.NewGuid();
//            var @event = CreateValidEvent(host.Id, eventId);
//            host.AddOrganizedEvent(eventId);
//            _mockEventRepository
//                .Setup(r => r.GetByIdAsync(eventId, new CancellationToken()))
//                .ReturnsAsync(@event);

//            _mockEventRepository
//                .Setup(r => r.DeleteAsync(eventId, new CancellationToken()))
//                .Returns(Task.CompletedTask);

//            _mockOrderRepository
//                .Setup(r => r.GetOrdersForEventAsync(eventId))
//                .ReturnsAsync(new List<Order>());

//            var result = await _service.RemoveEventFromHostAsync(host, eventId, new CancellationToken());

//            result.IsError.Should().BeFalse();
//            _mockHostRepository.Verify(r => r.UpdateAsync(host, new CancellationToken()), Times.Once);
//            _mockEventRepository.Verify(r => r.DeleteAsync(eventId, new CancellationToken()), Times.Once);
//        }


//        private Host CreateValidHost()
//        {
//            var contactInfo = new ContactInfo(
//                Guid.NewGuid(), "John", "Doe",
//                "111-222-3333", "john.doe@example.com", "john_doe");

//            return new Host(contactInfo, Guid.NewGuid());
//        }

//        private Event CreateValidEvent(Guid hostId, Guid eventId)
//        {
//            var date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));
//            var timeRangeResult = TimeRange.FromDateTimes(DateTime.UtcNow.AddDays(10).AddHours(12),
//                                                           DateTime.UtcNow.AddDays(10).AddHours(14));
//            timeRangeResult.IsError.Should().BeFalse();
//            var timeRange = timeRangeResult.Value;
//            return new Event(
//                "Concert",
//                "A great concert",
//                date,
//                timeRange,
//                Guid.NewGuid(),
//                hostId,
//                eventId);
//        }
//    }
//}
