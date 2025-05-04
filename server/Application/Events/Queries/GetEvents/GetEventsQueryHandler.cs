namespace Application.Events.Queries.GetEvents
{
    using Domain.Event;
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using ErrorOr;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetEventsQueryHandler: IRequestHandler<GetEventsQuery, ErrorOr<List<GetEventsOutputModel>>>
    {
        private readonly IEventDomainRepository _repository;
        public GetEventsQueryHandler(IEventDomainRepository repository) 
            => _repository = repository;

        public async Task<ErrorOr<List<GetEventsOutputModel>>> Handle(
            GetEventsQuery request,
            CancellationToken cancellationToken)
        {
            var events = await _repository.GetEventsByFilter(
                predicate: e => e.Status == EventStatus.Active,
                cancellationToken);

            var result = events
                .Select(e => new GetEventsOutputModel(
                    e.Id,
                    e.Name,
                    e.Description,
                    e.Date,
                    e.Time,
                    e.Status))
                .ToList();

            if (result is null)
            {
                return EventErrors.NoEventsFound;
            }

            return result;
        }
    }
}