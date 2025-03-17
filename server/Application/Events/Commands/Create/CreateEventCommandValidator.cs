namespace Application.Events.Commands.Create
{
    using FluentValidation;

    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
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

            this.RuleFor(e => e.HostId)
                .NotEmpty();

            this.RuleFor(e => e.Capacity)
                .GreaterThan(0);
        }
    }
}
