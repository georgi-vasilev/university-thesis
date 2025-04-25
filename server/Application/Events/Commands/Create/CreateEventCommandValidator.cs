namespace Application.Events.Commands.Create
{
    using FluentValidation;

    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
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

            this.RuleFor(e => e.Time)
                .NotNull()
                    .WithMessage("Time must be provided.");

            this.RuleFor(e => e.VenueId)
                .NotEmpty()
                    .WithMessage("Venue id must be provided");


            this.RuleFor(e => e.HostId)
                .NotEmpty()
                    .WithMessage("Host id must be provided");

            this.RuleFor(e => e.Capacity)
                .GreaterThan(0)
                    .WithMessage("Capacity cannot be 0");
        }
    }
}
