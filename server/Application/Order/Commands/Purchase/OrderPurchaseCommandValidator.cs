namespace Application.Order.Commands.Purchase
{
    using FluentValidation;

    public class OrderPurchaseCommandValidator : AbstractValidator<OrderPurchaseCommand>
    {
        public OrderPurchaseCommandValidator()
        {
            RuleFor(cmd => cmd.EventId)
                .NotEmpty()
                    .WithMessage("Event ID must be provided.");


            RuleFor(cmd => cmd.Quantity)
                .NotNull()
                    .WithMessage("Ticket quantity must be provided.")
                .NotEmpty();
        }
    }
}
