namespace Infrastructure.Services
{
    using Application.Services.Contracts.Payment;
    using Domain.Common.ValueObject;
    using ErrorOr;

    public class StripePaymentService : IPaymentService
    {
        public Task<ErrorOr<PaymentResult>> ProcessPaymentAsync(Money amount, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
