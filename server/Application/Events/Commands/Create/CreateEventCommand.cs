namespace Application.Events.Commands.Create
{
    using ErrorOr;
    using MediatR;

    public record CreateEventCommand(
        string Name,
        string Description,
        string ImageUrl,
        DateOnly Date,
        DateTime StartTime,
        DateTime EndTime,
        Guid VenueId,
        decimal GeneralTicketPrice,
        decimal? VipTicketPrice,
        int Capacity) : IRequest<ErrorOr<CreateEventOutputModel>>;
}
