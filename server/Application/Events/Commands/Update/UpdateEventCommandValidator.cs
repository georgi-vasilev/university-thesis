namespace Application.Events.Commands.Update
{
    using FluentValidation;

    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            this.RuleFor(e => e.Name)
                .MinimumLength(2)
                .MaximumLength(50)
                .NotEmpty();

            this.RuleFor(e => e.Description)
                .MinimumLength(10)
                .MaximumLength(250)
                .NotEmpty();

            this.RuleFor(e => e.Date)
                .GreaterThan(e => DateOnly.FromDateTime(DateTime.UtcNow.Date));

            this.RuleFor(e => e.Time)
                .NotNull();

            this.RuleFor(e => e.VenueId)
                .NotEmpty();
        }
    }
}
