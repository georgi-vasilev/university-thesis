namespace Application.Events.Commands.Cancel
{
    using FluentValidation;

    public class CancelEventCommandValidator : AbstractValidator<CancelEventCommand>
    {
        public CancelEventCommandValidator()
        {
            this.RuleFor(x => x.EventId)
                .NotEmpty()
                    .WithMessage("Event id must be provided");

            this.RuleFor(x => x.HostId)
                .NotEmpty()
                    .WithMessage("Host id must be provided");

            this.RuleFor(x => x.Status)
                .NotNull()
                    .WithMessage("Status must be provided")
                .IsInEnum()
                    .WithMessage("Status must be valid.");
        }
    }
}
