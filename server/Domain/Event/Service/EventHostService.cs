namespace Domain.Event.Service
{
    using Error;
    using ErrorOr;
    using Host;
    using Host.Repository;
    using Order;
    using Order.Repository;
    using Repository;

    internal class EventHostService : IEventHostService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IHostRepository _hostRepository;
        private readonly IOrderRepository _orderRepository;

        public EventHostService(IEventRepository eventRepository, IHostRepository hostRepository, IOrderRepository orderRepository)
        {
            _eventRepository = eventRepository;
            _hostRepository = hostRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ErrorOr<Success>> CancelEventForHostAsync(Host host, Guid eventId)
        {

            var @event = await _eventRepository.GetByIdAsync(eventId);  
            if (@event == null)
            {
                return EventErrors.EventNotFoundError;
            }

            if (host.Id != @event.HostId)
            {
                return EventErrors.EventDoesNotBelongToHostError;
            }

            var changeResult = @event.ChangeStatus(EventStatus.Cancelled);
            if (changeResult.IsError)
            {
                return changeResult.FirstError;
            }

            await _eventRepository.UpdateAsync(@event);

            //TODO: dispatch event
            return Result.Success;
        }

        public async Task<ErrorOr<Success>> CreateEventForHostAsync(Host host, Event @event)
        {
            if (host.Id != @event.HostId)
            {
                return EventErrors.EventDoesNotBelongToHostError;
            }

            await _eventRepository.AddAsync(@event);

            var addResult = host.AddOrganizedEvent(@event.Id);
            if (addResult.IsError)
            {
                return addResult.FirstError;
            }

            await _hostRepository.UpdateAsync(host);

            // TODO: dispatch event
            return Result.Success;
        }

        public async Task<ErrorOr<Success>> RemoveEventFromHostAsync(Host host, Guid eventId)
        {
            var @event = await _eventRepository.GetByIdAsync(eventId);
            if (@event is null)
            {
                return EventErrors.EventNotFoundError;
            }

            if (host.Id != @event.HostId)
            {
                return EventErrors.EventDoesNotBelongToHostError;
            }

            var orders = await _orderRepository.GetOrdersForEventAsync(eventId);
            if (orders.Any(o => o.Status == OrderStatus.Completed))
            {
                return EventErrors.CannotDeleteEventWithOrders;
            }

            var removeResult = host.DeleteOrganizedEvent(eventId);
            if (removeResult.IsError)
            {
                return removeResult.FirstError;
            }

            await _hostRepository.UpdateAsync(host);
            await _eventRepository.DeleteAsync(eventId);

            return Result.Success;
        }
    }
}