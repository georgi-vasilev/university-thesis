namespace Application.Events.Queries.GetEventsInDateRange
{
    using ErrorOr;
    using MediatR;

    public record GetEventsInDateRangeQuery : IRequest<ErrorOr<IEnumerable<GetEventsInDateRangeOutputModel>>>
    {
        public GetEventsInDateRangeQuery(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
