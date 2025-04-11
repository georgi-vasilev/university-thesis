namespace Application.Host.Commands.Create
{
    using FluentValidation;

    public class CreateHostCommandValidator : AbstractValidator<CreateHostCommand>
    {
        public CreateHostCommandValidator()
        {
            this.RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(15);

            this.RuleFor(x => x.LastName)
             .NotEmpty()
             .MinimumLength(3)
             .MaximumLength(15);

            this.RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MinimumLength(9) // without prefix
                .MaximumLength(12); // with country code

            this.RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            this.RuleFor(x => x.InstagramHandler)
                .NotEmpty()
                .MinimumLength(3);
        }
    }
}
