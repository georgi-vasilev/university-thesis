namespace Application.Events.Queries.GetEventsByHost
{
    using GetEvents;
    using ErrorOr;
    using MediatR;
    using Application.Common.Contracts;

    public class GetEventsByHostQueryHandler : IRequestHandler<GetEventsByHostQuery, ErrorOr<List<GetEventsOutputModel>>>
    {
        private readonly IEventQueryRepository _repository;

        public GetEventsByHostQueryHandler(IEventQueryRepository repository) 
            => _repository = repository;

        public async Task<ErrorOr<List<GetEventsOutputModel>>> Handle(GetEventsByHostQuery request, CancellationToken cancellationToken)
            => await _repository.GetEventsByHostAsync(request.Id, cancellationToken);
    }
}
