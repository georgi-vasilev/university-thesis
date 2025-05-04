namespace Application.Events.Queries.GetEventDetails
{
    using FluentValidation;

    public class GetEventDetailsQueryValidator : AbstractValidator<GetEventDetailsQuery>
    {
        public GetEventDetailsQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Event ID cannot be empty.");
        }
    }
}
