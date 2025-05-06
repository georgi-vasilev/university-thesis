namespace Application.Events.Queries.GetEventsByVenue
{
    using FluentValidation;

    public class GetEventsByVenueQueryValidator : AbstractValidator<GetEventsByVenueQuery>
    {
        public GetEventsByVenueQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Venue ID cannot be empty.");
        }
    }
}
