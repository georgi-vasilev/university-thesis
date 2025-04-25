namespace Application.Order.Commands.Create
{
    using FluentValidation;
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(cmd => cmd.BuyerId)
                .NotEmpty()
                    .WithMessage("Buyer ID must be provided.");
        }
    }
}
