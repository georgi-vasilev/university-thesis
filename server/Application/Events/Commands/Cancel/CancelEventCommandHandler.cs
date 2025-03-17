namespace Application.Events.Commands.Cancel
{
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using Domain.Host.Error;
    using Domain.Host.Repository;
    using ErrorOr;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, ErrorOr<Success>>
    {
        private readonly IHostDomainRepository _hostRepository;
        private readonly IEventDomainRepository _eventRepository;

        public CancelEventCommandHandler(IHostDomainRepository repository, IEventDomainRepository eventRepository)
        {
            _hostRepository = repository;
            _eventRepository = eventRepository;
        }

        public async Task<ErrorOr<Success>> Handle(
            CancelEventCommand request,
            CancellationToken cancellationToken)
        {
            var host = await _hostRepository.GetByIdAsync(request.HostId, cancellationToken);

            if(host is null)
            {
                return HostErrors.HostNotFoundError;
            }

            if (!host.OrganizedEventIds.Any(id => id == request.EventId))
            {
                return HostErrors.EventDoesNotExistError;
            }

            var @event = await _eventRepository.GetEventByFilterAsync(
                predicate: e => e.HostId == request.HostId && e.Id == request.EventId,
                cancellationToken);

            if (@event is null)
            {
                return EventErrors.EventNotFoundError;
            }

            var changeStatusResult = @event.ChangeStatus(request.Statue);

            if (changeStatusResult.IsError)
            {
                return changeStatusResult.FirstError;
            }

            return Result.Success;
        }
    }
}
