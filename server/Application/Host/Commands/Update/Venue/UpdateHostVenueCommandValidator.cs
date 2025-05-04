namespace Application.Host.Commands.Update.Venue
{
    using FluentValidation;

    public class UpdateHostVenueCommandValidator : AbstractValidator<UpdateHostVenueCommand>
    {
        public UpdateHostVenueCommandValidator()
        {
            this.RuleFor(host => host.VenueId)
                .NotEmpty()
                    .WithMessage("Venue id must be provided");
        }
    }
}
