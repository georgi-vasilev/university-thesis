namespace Application.Events.Queries.GetEventsByVenue
{
    using Application.Common.Contracts;
    using ErrorOr;
    using GetEvents;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetEventsByVenueQueryHandler : IRequestHandler<GetEventsByVenueQuery, ErrorOr<List<GetEventsOutputModel>>>
    {
        private readonly IEventQueryRepository _repository;

        public GetEventsByVenueQueryHandler(IEventQueryRepository repository)
            => _repository = repository;

        public Task<ErrorOr<List<GetEventsOutputModel>>> Handle(GetEventsByVenueQuery request, CancellationToken cancellationToken) 
            => _repository.GetEventsByVenueAsync(request.Id, cancellationToken);
    }
}
