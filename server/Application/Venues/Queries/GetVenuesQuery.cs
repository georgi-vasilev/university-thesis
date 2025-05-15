namespace Application.Venues.Queries
{
    using ErrorOr;
    using MediatR;

    public record GetVenuesQuery : IRequest<ErrorOr<IEnumerable<GetVenuesOutputModel>>>;
}
