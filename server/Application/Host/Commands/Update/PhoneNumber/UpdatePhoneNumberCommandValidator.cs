namespace Application.Host.Commands.Update.PhoneNumber
{
    using FluentValidation;

    public class UpdatePhoneNumberCommandValidator : AbstractValidator<UpdatePhoneNumberCommand>
    {
        public UpdatePhoneNumberCommandValidator()
        {
            this.RuleFor(host => host.Id)
                .NotEmpty()
                    .WithMessage("Host id must be provided");

            this.RuleFor(host => host.PhoneNumber)
                .NotEmpty()
                    .WithMessage("Phone number cannot be empty string.")
                .MinimumLength(9) // without prefix
                    .WithMessage("Phone has minimum length of 9 characters.")
                .MaximumLength(12) // with country code
                    .WithMessage("Phone has maximum length of 12 characters.");
        }
    }
}
