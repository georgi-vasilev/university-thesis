namespace Application.Events.Queries.GetEventsByHost
{
    using Application.Common.Contracts;
    using Application.Common.Models;
    using Domain.Event.Error;
    using ErrorOr;
    using GetEvents;
    using MediatR;
    using Services.Contracts.User;

    public class GetEventsByHostQueryHandler : IRequestHandler<GetEventsByHostQuery, ErrorOr<PaginatedResult<GetEventsOutputModel>>>
    {
        private readonly IEventQueryRepository _repository;
        private readonly ICurrentUser _currentUser;

        public GetEventsByHostQueryHandler(IEventQueryRepository repository, ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<PaginatedResult<GetEventsOutputModel>>> Handle(GetEventsByHostQuery request, CancellationToken cancellationToken)
        {
            if (_currentUser.HostId is null)
            {
                return EventErrors.Unauthorized;
            }

            var hostId = _currentUser.HostId.Value;
            return await _repository.GetEventsByHostAsync(
                        hostId,
                        request.PageIndex,
                        request.PageSize,
                        request.Ordering,
                        cancellationToken);
        }
    }
}
