namespace Application.Order.Commands.Purchase
{
    using FluentValidation;

    public class OrderPurchaseCommandValidator : AbstractValidator<OrderPurchaseCommand>
    {
        public OrderPurchaseCommandValidator()
        {
            RuleFor(cmd => cmd.BuyerId)
                .NotEmpty()
                    .WithMessage("Buyer ID must be provided.");

            RuleFor(cmd => cmd.EventId)
                .NotEmpty()
                    .WithMessage("Event ID must be provided.");


            RuleFor(cmd => cmd.PaymentAmount)
                .NotNull()
                    .WithMessage("PaymentAmount must be provided.")
                .NotEmpty()
                .Must(x => x.Amount > 0)
                    .WithMessage("Amoumt must be more than 0.")
                .Must(x => string.IsNullOrEmpty(x.Currency))
                    .WithMessage("Currency must be provided.");
        }
    }
}
