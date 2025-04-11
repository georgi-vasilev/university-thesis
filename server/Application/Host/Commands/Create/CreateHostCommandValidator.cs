namespace Application.Host.Commands.Create
{
    using FluentValidation;

    public class CreateHostCommandValidator : AbstractValidator<CreateHostCommand>
    {
        public CreateHostCommandValidator()
        {
            this.RuleFor(host => host.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(15);

            this.RuleFor(host => host.LastName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(15);

            this.RuleFor(host => host.PhoneNumber)
                .NotEmpty()
                .MinimumLength(9) // without prefix
                .MaximumLength(12); // with country code

            this.RuleFor(host => host.Email)
                .NotEmpty()
                .EmailAddress();

            this.RuleFor(host => host.InstagramHandler)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);
        }
    }
}
