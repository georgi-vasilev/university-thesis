namespace Application.Host.Commands.Create
{
    using FluentValidation;

    public class CreateHostCommandValidator : AbstractValidator<CreateHostCommand>
    {
        public CreateHostCommandValidator()
        {
            this.RuleFor(host => host.FirstName)
                .NotEmpty()
                    .WithMessage("First name cannot be empty string.")
                .MinimumLength(3)
                    .WithMessage("First name has minimum length of 3 characters.")
                .MaximumLength(15)
                    .WithMessage("First name has maximum length of 15 characters.");

            this.RuleFor(host => host.LastName)
                .NotEmpty()
                    .WithMessage("Last name cannot be empty string.")
                .MinimumLength(3)
                    .WithMessage("Last name has minimum length of 3 characters.")
                .MaximumLength(15)
                    .WithMessage("First name has maximum length of 15 characters.");

            this.RuleFor(host => host.PhoneNumber)
                .NotEmpty()
                    .WithMessage("Phone number cannot be empty string.")
                .MinimumLength(9) // without prefix
                    .WithMessage("Phone has minimum length of 9 characters.")
                .MaximumLength(12) // with country code
                    .WithMessage("Phone has maximum length of 12 characters.");

            this.RuleFor(host => host.Email)
                .NotEmpty()
                    .WithMessage("Email cannot be empty string.")
                .EmailAddress()
                    .WithMessage("Email must be valid email address.");

            this.RuleFor(host => host.InstagramHandler)
                .NotEmpty()
                    .WithMessage("Instagram handler cannot be empty string.")
                .MinimumLength(3)
                    .WithMessage("Instagram handler has minimum of lenght of 3 characters.")
                .MaximumLength(30)
                    .WithMessage("Instagram handler has maximum of lenght of 30 characters.");
        }
    }
}
