namespace Application.Host.Commands.Update.InstagramHandler
{
    using FluentValidation;

    public class UpdateInstagramHandlerCommandValidator : AbstractValidator<UpdateInstagramHandlerCommand>
    {
        public UpdateInstagramHandlerCommandValidator()
        {
            this.RuleFor(host => host.Id)
                .NotEmpty()
                    .WithMessage("Host id must be provided");

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
