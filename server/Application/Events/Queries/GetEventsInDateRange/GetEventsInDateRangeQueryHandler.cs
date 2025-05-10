namespace Application.Events.Queries.GetEventsInDateRange
{
    using Application.Common.Contracts;
    using Application.Common.Models;
    using Domain.Event.Error;
    using ErrorOr;
    using MediatR;

    public class GetEventsInDateRangeQueryHandler : IRequestHandler<GetEventsInDateRangeQuery, ErrorOr<PaginatedResult<GetEventsInDateRangeOutputModel>>>
    {
        private IEventQueryRepository _repository;

        public GetEventsInDateRangeQueryHandler(IEventQueryRepository repository)
            => _repository = repository;

        public async Task<ErrorOr<PaginatedResult<GetEventsInDateRangeOutputModel>>> Handle(
        GetEventsInDateRangeQuery request,
        CancellationToken cancellationToken)
        {
            var fromUtc = request.StartDate.ToUniversalTime();
            var toUtc = request.EndDate.ToUniversalTime();

            var paginatedEvents = await _repository.GetEventsInDateRangeAsync(
                fromUtc,
                toUtc,
                request.PageIndex,
                request.PageSize,
                request.Ordering,
                cancellationToken);

            if (paginatedEvents.TotalCount == 0)
            {
                return EventErrors.NoEventsFoundInTheGivenTimeRangeError;
            }

            var output = paginatedEvents
                .Items
                .Select(e => new GetEventsInDateRangeOutputModel(
                    e.Id,
                    e.Name,
                    e.Description,
                    e.Date,
                    e.Time,
                    e.Status.ToString()))
                .ToList();

            return new PaginatedResult<GetEventsInDateRangeOutputModel>(
                output,
                paginatedEvents.PageIndex,
                paginatedEvents.PageSize,
                paginatedEvents.TotalCount);
        }
    }
}
