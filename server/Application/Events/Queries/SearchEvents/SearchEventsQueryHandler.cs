namespace Application.Events.Queries.SearchEvents
{
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using ErrorOr;
    using GetEvents;
    using MediatR;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class SearchEventsQueryHandler : IRequestHandler<SearchEventsQuery, ErrorOr<List<GetEventsOutputModel>>>
    {
        private IEventDomainRepository _repository;

        public SearchEventsQueryHandler(IEventDomainRepository repository) 
            => _repository = repository;

        public async Task<ErrorOr<List<GetEventsOutputModel>>> Handle(SearchEventsQuery request, CancellationToken cancellationToken)
        {
            var pattern = $@"\b{Regex.Escape(request.SearchTerm)}\b";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            var events = await _repository.GetEventsByFilterAsync(
                predicate: e =>
                    e.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    regex.IsMatch(e.Description),
                cancellationToken);

            var result = events
                .Select(e => new GetEventsOutputModel(
                    e.Id,
                    e.Name,
                    e.Description,
                    e.Date,
                    e.Time,
                    e.Status.ToString()))
                .ToList();

            if (result.Count == 0)
            {
                return EventErrors.NoEventsFoundError;
            }

            return result;
        }
    }
}
