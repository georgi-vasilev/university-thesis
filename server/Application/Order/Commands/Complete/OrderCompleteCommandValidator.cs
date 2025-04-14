namespace Application.Order.Commands.Complete
{
    using FluentValidation;

    public class OrderCompleteCommandValidator : AbstractValidator<OrderCompleteCommand>
    {
        public OrderCompleteCommandValidator()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty()
                .WithMessage("Order ID must be provided."); //TODO: Add messages for the rest of the validators.
        }
    }
}
