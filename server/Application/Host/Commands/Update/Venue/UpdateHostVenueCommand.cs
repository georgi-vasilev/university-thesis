namespace Application.Host.Commands.Update.Venue
{
    using ErrorOr;
    using MediatR;

    public record UpdateHostVenueCommand(Guid VenueId) : IRequest<ErrorOr<UpdateHostVenueOutputModel>>;
}