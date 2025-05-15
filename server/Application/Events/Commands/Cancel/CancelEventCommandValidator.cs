namespace Application.Events.Commands.Cancel
{
    using FluentValidation;

    public class CancelEventCommandValidator : AbstractValidator<CancelEventCommand>
    {
        public CancelEventCommandValidator()
        {
            this.RuleFor(x => x.Id)
                .NotEmpty()
                    .WithMessage("Event id must be provided");
        }
    }
}
