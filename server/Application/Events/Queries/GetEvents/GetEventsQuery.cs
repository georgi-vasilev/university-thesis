namespace Application.Events.Queries.GetEvents
{
    using Application.Common.Models;
    using Common;
    using ErrorOr;
    using MediatR;

    public record GetEventsQuery(int PageIndex, int PageSize, EventOrdering Ordering) : IRequest<ErrorOr<PaginatedResult<GetEventsOutputModel>>>;

}
