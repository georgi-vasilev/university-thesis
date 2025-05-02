namespace Application.Events.Commands.Update
{
    using FluentValidation;

    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            this.RuleFor(e => e.Name)
                 .NotEmpty()
                     .WithMessage("Name cannot be empty string.")
                 .MinimumLength(2)
                     .WithMessage("Name has minimum length of 2 characters.")
                 .MaximumLength(50)
                     .WithMessage("Name has maximum length of 50 characters.");

            this.RuleFor(e => e.Description)
                .NotEmpty()
                    .WithMessage("Description cannot be empty string.")
                .MinimumLength(10)
                    .WithMessage("Description has minimum length of 10 characters.")
                .MaximumLength(250)
                    .WithMessage("Description has maximum length of 250 characters.");

            this.RuleFor(e => e.Date)
                .GreaterThan(e => DateOnly.FromDateTime(DateTime.UtcNow.Date))
                    .WithMessage("Date must be in the future. Cannot enter past date");

            this.RuleFor(e => e.StartTime)
                .NotNull()
                    .WithMessage("Start time must be provided.")
                .LessThan(e => e.EndTime)
                    .WithMessage("End time cannot be before start time");

            this.RuleFor(e => e.EndTime)
                .NotNull()
                    .WithMessage("End time must be provided.")
                .GreaterThan(e => e.StartTime)
                    .WithMessage("End time cannot be before start time");

            this.RuleFor(e => e.VenueId)
                .NotEmpty()
                    .WithMessage("Venue id must be provided");
        }
    }
}
