namespace Application.Host.Commands.Update.Email
{
    using FluentValidation;

    public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
    {
        public UpdateEmailCommandValidator()
        {
            this.RuleFor(host => host.Email)
               .NotEmpty()
                   .WithMessage("Email cannot be empty string.")
               .EmailAddress()
                   .WithMessage("Email must be valid email address.");
        }
    }
}
