namespace Application.Events.Queries.GetEventsByHost
{
    using FluentValidation;

    public class GetEventsByHostQueryValidator : AbstractValidator<GetEventsByHostQuery>
    {
        public GetEventsByHostQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Host ID cannot be empty.");
        }
    }
}
