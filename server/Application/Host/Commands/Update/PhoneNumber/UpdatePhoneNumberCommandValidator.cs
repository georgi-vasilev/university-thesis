namespace Application.Host.Commands.Update.PhoneNumber
{
    using FluentValidation;

    public class UpdatePhoneNumberCommandValidator : AbstractValidator<UpdatePhoneNumberCommand>
    {
        public UpdatePhoneNumberCommandValidator()
        {
            this.RuleFor(host => host.Id)
                .NotEmpty();

            this.RuleFor(host => host.PhoneNumber)
                .NotEmpty()
                .MinimumLength(9) // without prefix
                .MaximumLength(12); // with country code
        }
    }
}
