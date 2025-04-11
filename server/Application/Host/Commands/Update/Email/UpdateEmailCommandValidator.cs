using FluentValidation;

namespace Application.Host.Commands.Update.Email
{
    public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
    {
        public UpdateEmailCommandValidator()
        {
            this.RuleFor(host => host.Id)
                .NotEmpty();

            this.RuleFor(host => host.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
