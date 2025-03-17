namespace Application.Events.Commands.Cancel
{
    using FluentValidation;

    public class CancelEventCommandValidator : AbstractValidator<CancelEventCommand>
    {
        public CancelEventCommandValidator()
        {
            this.RuleFor(x => x.EventId).NotEmpty();
            this.RuleFor(x => x.HostId).NotEmpty();
            this.RuleFor(x => x.Statue)
                .NotNull()
                .IsInEnum();
        }
    }
}
