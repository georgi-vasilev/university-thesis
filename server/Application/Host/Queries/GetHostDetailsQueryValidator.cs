namespace Application.Host.Queries
{
    using FluentValidation;

    public class GetHostDetailsQueryValidator : AbstractValidator<GetHostDetailsQuery>
    {
        public GetHostDetailsQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Host ID cannot be empty.");
        }
    }
}
