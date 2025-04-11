namespace Application.Host.Commands.Update.InstagramHandler
{
    using FluentValidation;

    public class UpdateInstagramHandlerCommandValidator : AbstractValidator<UpdateInstagramHandlerCommand>
    {
        public UpdateInstagramHandlerCommandValidator()
        {
            this.RuleFor(host => host.Id)
                .NotEmpty();

            this.RuleFor(host => host.InstagramHandler)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);
        }
    }
}
