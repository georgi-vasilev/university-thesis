namespace Application.Events.Queries.GetEventsInDateRange
{
    using Application.Common.Models;
    using Common;
    using ErrorOr;
    using MediatR;

    public record GetEventsInDateRangeQuery(
        DateTime StartDate,
        DateTime EndDate,
        int PageSize,
        int PageIndex,
        EventOrdering Ordering) : IRequest<ErrorOr<PaginatedResult<GetEventsInDateRangeOutputModel>>>;
}
