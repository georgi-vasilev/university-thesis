namespace Application.Events.Queries.GetEventsByHost
{
    using Application.Common.Models;
    using Application.Events.Common;
    using ErrorOr;
    using GetEvents;
    using MediatR;

    public record GetEventsByHostQuery(int PageIndex, int PageSize, EventOrdering Ordering) : IRequest<ErrorOr<PaginatedResult<GetEventsOutputModel>>>;
}
