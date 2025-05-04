namespace Application.Events.Queries.GetEvents
{
    using ErrorOr;
    using MediatR;

    public record GetEventsQuery() : IRequest<ErrorOr<List<GetEventsOutputModel>>>;
}
