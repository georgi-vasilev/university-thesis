namespace Application.Events.Queries.GetEvents
{
    using Application.Common.Contracts;
    using Application.Common.Models;
    using Domain.Event.Error;
    using ErrorOr;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, ErrorOr<PaginatedResult<GetEventsOutputModel>>>
    {
        private readonly IEventQueryRepository _repository;
        public GetEventsQueryHandler(IEventQueryRepository repository)
            => _repository = repository;

        public async Task<ErrorOr<PaginatedResult<GetEventsOutputModel>>> Handle(
         GetEventsQuery request,
         CancellationToken cancellationToken)
        {
            var paginatedEvents = await _repository.GetActiveEventsAsync(
                request.PageIndex,
                request.PageSize,
                request.Ordering,
                cancellationToken);

            if (paginatedEvents.TotalCount == 0)
            {
                return EventErrors.NoEventsFoundError;
            }

            var output = paginatedEvents
                .Items
                .Select(e => new GetEventsOutputModel(
                    e.Id,
                    e.Name,
                    e.Description,
                    e.Date,
                    e.Time,
                    e.Status.ToString()))
                .ToList();

            return new PaginatedResult<GetEventsOutputModel>(
                output,
                paginatedEvents.PageIndex,
                paginatedEvents.PageSize,
                paginatedEvents.TotalCount);
        }
    }
}