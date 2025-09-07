namespace Application.Order.Commands.Complete
{
    using FluentValidation;

    public class OrderCompleteCommandValidator : AbstractValidator<OrderCompleteCommand>
    {
        public OrderCompleteCommandValidator()
        {
            RuleFor(c => c.TranscationId)
                .NotEmpty()
                    .WithMessage("Transcation ID must be provided.");
        }
    }
}
