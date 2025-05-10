namespace Application.Events.Queries.GetEventsInDateRange
{
    using Domain.Event.Error;
    using Domain.Event.Repository;
    using ErrorOr;
    using MediatR;

    public class GetEventsInDateRangeQueryHandler : IRequestHandler<GetEventsInDateRangeQuery, ErrorOr<IEnumerable<GetEventsInDateRangeOutputModel>>>
    {
        private IEventDomainRepository _repository;

        public GetEventsInDateRangeQueryHandler(IEventDomainRepository repository)
            => _repository = repository;

        public async Task<ErrorOr<IEnumerable<GetEventsInDateRangeOutputModel>>> Handle(GetEventsInDateRangeQuery request, CancellationToken cancellationToken)
        {
            var fromUtc = request.StartDate.ToUniversalTime();
            var toUtc = request.EndDate.ToUniversalTime();
            var events = await _repository.GetEventsByFilterAsync(
                predicate: e => e.Time.Start >= fromUtc && e.Time.End <= toUtc,
                cancellationToken);

            if (events is null)
            {
                return EventErrors.NoEventsFoundInTheGivenTimeRangeError;
            }

            if (events.Count() == 0)
            {
                return EventErrors.NoEventsFoundInTheGivenTimeRangeError;
            }

            var output = events
                .Select(e => new GetEventsInDateRangeOutputModel(
                    e.Id,
                    e.Name,
                    e.Description,
                    e.Date,
                    e.Time,
                    e.Status.ToString()))
                .ToList();

            return output;
        }
    }
}
